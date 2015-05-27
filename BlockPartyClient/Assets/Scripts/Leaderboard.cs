using UnityEngine;
using System.Collections;
using BlockPartyShared;
using System.Collections.Generic;

public class Leaderboard : MonoBehaviour
{
    static Leaderboard instance;
    public static Leaderboard Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Leaderboard>();
                
                // tell unity not to destroy this object when loading a new scene
                DontDestroyOnLoad(instance.gameObject);
            }
            
            return instance;
        }
    }

    public List<KeyValuePair<string, int>> SortedLeaderboard;

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
    }

    // Use this for initialization
    void Start()
    {
        NetworkingManager.Instance.MessageReceived += networkingManager_MessageReceived;
        
        if (!NetworkingManager.Instance.Connected)
        {
            NetworkingManager.Instance.Connect();
        }
    }
	
    void networkingManager_MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        if (e.Message.Type == NetworkMessage.MessageType.ServerLeaderboard)
        {
            SortedLeaderboard = (List<KeyValuePair<string, int>>)e.Message.Content;
        }
    }

    // Update is called once per frame
    void Update()
    {
	
    }
}
