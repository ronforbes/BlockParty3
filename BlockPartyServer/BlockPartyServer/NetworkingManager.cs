using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using BlockPartyShared;

namespace BlockPartyServer
{
	public class NetworkingManager
	{
		TcpListener listener;
		List<TcpClient> clients;
		BinaryFormatter formatter;
		const int port = 1337;
		
		public event EventHandler<MessageReceivedEventArgs> MessageReceived;
		
		public NetworkingManager ()
		{
			// Initialize the TCP listener at the Azure instance endpoint
			IPEndPoint endpoint = new IPEndPoint (IPAddress.Any, port);
			
			listener = new TcpListener (endpoint);
			listener.Start ();
			
			Console.WriteLine ("Started listening at {0}", endpoint.ToString ());
			
			// Initialize the TCP client list
			clients = new List<TcpClient> ();
			formatter = new BinaryFormatter ();
			
			// Start accepting new connections from TCP clients
			listener.BeginAcceptTcpClient (new AsyncCallback (OnAcceptTcpClient), listener);
		}
		
		void OnAcceptTcpClient (IAsyncResult result)
		{
			TcpListener resultingListener = (TcpListener)result.AsyncState;
			
			TcpClient client = resultingListener.EndAcceptTcpClient (result);
			
			Console.WriteLine ("Accepted new client at {0}", client.Client.RemoteEndPoint.ToString ());
			clients.Add (client);
			
			// Start receiving data from this client
			Stream stream = client.GetStream ();
			if (stream.CanRead) {
				Thread receiveThread = new Thread (Receive);
				receiveThread.Start (client);
			}
			
			// Start accepting more new TCP clients
			listener.BeginAcceptTcpClient (new AsyncCallback (OnAcceptTcpClient), listener);
		}
		
		void Receive (object parameter)
		{
			TcpClient client = (TcpClient)parameter;
			NetworkStream stream = client.GetStream ();
			
			while (true) {
				try {
					NetworkMessage message = (NetworkMessage)formatter.Deserialize (stream);
					Console.WriteLine ("Received message from client at {0}: {1}", client.Client.RemoteEndPoint.ToString (), message.ToString ());
					
					// process message
					MessageReceivedEventArgs args = new MessageReceivedEventArgs ();
					args.Message = message;
					args.Sender = client;
					OnMessageReceived (args);
				} catch (IOException e) {
					Console.WriteLine ("Client disconnected from {0} (Exception: {1})", client.Client.RemoteEndPoint.ToString (), e.ToString ());
					clients.Remove (client);
					client.Close ();
					return;
				}
			}
		}
		
		protected virtual void OnMessageReceived (MessageReceivedEventArgs e)
		{
			EventHandler<MessageReceivedEventArgs> handler = MessageReceived;
			if (handler != null) {
				handler (this, e);
			}
		}
		
		public void Send (TcpClient client, NetworkMessage message)
		{
			NetworkStream stream = client.GetStream ();
			
			formatter.Serialize (stream, message);
			
			Console.WriteLine ("Sent message to client at {0}: {1}", client.Client.RemoteEndPoint.ToString (), message.ToString ());
		}
		
		public void Broadcast (NetworkMessage message)
		{
			foreach (TcpClient client in clients) {
				Send (client, message);
			}
			
			Console.WriteLine ("Broadcasted message to all clients: {0}", message.ToString ());
		}
	}
}
