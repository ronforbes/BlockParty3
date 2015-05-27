using UnityEngine;
using System.Collections;
using BlockPartyShared;

public class ResultsSubmitter : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        NetworkingManager.Instance.Send(new NetworkMessage(NetworkMessage.MessageType.ClientResults, StatsTracker.Instance.Score));
    }
	
    // Update is called once per frame
    void Update()
    {

    }
}
