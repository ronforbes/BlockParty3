using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class TimeRenderer : MonoBehaviour
{
    Text text;

    // Use this for initialization
    void Start()
    {
        text = GetComponent<Text>();
    }
	
    // Update is called once per frame
    void Update()
    {
        if (GameStateTimer.Instance)
        {
            int minutes = (int)(GameStateTimer.Instance.TimeRemaining / 60.0f);
            int seconds = (int)(GameStateTimer.Instance.TimeRemaining % 60.0f);
            text.text = String.Format("{0}:{1:00}", minutes, seconds);
        }
    }
}
