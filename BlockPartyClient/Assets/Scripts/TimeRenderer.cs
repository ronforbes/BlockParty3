using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimeRenderer : MonoBehaviour
{
    Text text;
    GameStateTimer timer;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
        timer = GameObject.Find("Game State Timer").GetComponent<GameStateTimer>();
    }
	
    // Update is called once per frame
    void Update()
    {
        int minutes = (int)(timer.TimeRemaining / 60.0f);
        int seconds = (int)(timer.TimeRemaining % 60.0f);
        text.text = String.Format("{0}:{1:00}", minutes, seconds);
    }
}
