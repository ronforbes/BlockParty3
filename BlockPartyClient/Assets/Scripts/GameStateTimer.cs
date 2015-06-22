using UnityEngine;
using System.Collections;
using BlockPartyShared;

public class GameStateTimer : MonoBehaviour
{
    public enum GameState
    {
        None,
        Lobby,
        Game,
        Results,       
    }

    static GameStateTimer instance;
    public static GameStateTimer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameStateTimer>();
                
                // tell unity not to destroy this object when loading a new scene
                if (instance != null)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            
            return instance;
        }
    }

    public GameState State;
    GameState stateToLoad;
    public bool Playing;
    public float TimeRemaining;

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
        if (e.Message.Type == NetworkMessage.MessageType.ServerGameState)
        {
            ServerGameStateContent content = (ServerGameStateContent)e.Message.Content;
            TimeRemaining = content.TimeRemaining;
            switch (content.GameState)
            {   
                case "Lobby":              
                    State = GameState.Lobby;
                    Leaderboard.Instance.SortedLeaderboard = null;
                    break;

                case "Game":
                    State = GameState.Game;
                    StatsTracker.Instance.Reset();
                    break;

                case "Results":
                    State = GameState.Results;
                    break;
            }

            if (Playing)
            {
                stateToLoad = State;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TimeRemaining -= Time.deltaTime;

        if (Playing && stateToLoad != GameState.None)
        {
            string level = stateToLoad.ToString();
            stateToLoad = GameState.None;
            Application.LoadLevel(level);
        }
    }
}
