using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;
using System.Threading;
using BlockPartyShared;

public class NetworkingManager : MonoBehaviour
{
    static NetworkingManager instance;
    public static NetworkingManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<NetworkingManager>();

                // tell unity not to destroy this object when loading a new scene
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    TcpClient client;
    NetworkStream stream;
    BinaryFormatter formatter;
    //readonly string hostname = "localhost";
    readonly string hostname = "52.8.64.92";
    readonly int port = 1337;
    
    public event EventHandler<MessageReceivedEventArgs> MessageReceived;
    
    public bool Connected
    {
        get
        {
            return client != null && client.Connected;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            // if i'm the first instance, make me the singleton
            instance = this;
            DontDestroyOnLoad(this);
        } else
        {
            // if a singleton already exists and you find another reference in scene, destroy it
            if (this != instance)
            {
                Destroy(this.gameObject);
            }
        }

        // Forces a different code path in the BinaryFormatter that doesn't rely on run-time code generation (which would break on iOS).
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
    }

    public bool Connect()
    {
        try
        {
            client = new TcpClient(hostname, port);
        } catch (SocketException e)
        {
            return false;
        }
        
        if (client.Connected)
        {
            Debug.Log("Connected to server " + client.Client.RemoteEndPoint.ToString());
            
            stream = client.GetStream();
            formatter = new BinaryFormatter();
            
            if (stream.CanRead)
            {
                Thread receiveThread = new Thread(Receive);
                receiveThread.Start();
            }
            
            Send(new NetworkMessage(NetworkMessage.MessageType.ClientCreateUser, UserManager.Instance.Name));

            return true;
        }

        return false;
    }
    
    public void Disconnect()
    {
        client.Close();
        Debug.Log("Disconnected from server");
    }
    
    void Receive()
    {
        while (true)
        {
            NetworkMessage message = (NetworkMessage)formatter.Deserialize(stream);
            Debug.Log("Received message from server: " + message.ToString());
            
            // process message
            MessageReceivedEventArgs args = new MessageReceivedEventArgs();
            args.Message = message;
            OnMessageReceived(args);
        }
    }
    
    void OnMessageReceived(MessageReceivedEventArgs e)
    {
        EventHandler<MessageReceivedEventArgs> handler = MessageReceived;
        if (handler != null)
        {
            handler(null, e);
        }
    }
    
    public void Send(NetworkMessage message)
    {
        formatter.Serialize(stream, message);
        Debug.Log("Sent message to server: " + message.ToString());
    }
}