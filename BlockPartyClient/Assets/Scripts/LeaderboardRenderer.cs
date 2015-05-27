using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LeaderboardRenderer : MonoBehaviour
{
    Text rankText, nameText, scoreText;

    // Use this for initialization
    void Start()
    {
        rankText = GameObject.Find("Rank Value").GetComponent<Text>();
        nameText = GameObject.Find("Name Value").GetComponent<Text>();
        scoreText = GameObject.Find("Score Value").GetComponent<Text>();
    }
	
    // Update is called once per frame
    void Update()
    {
        if (Leaderboard.Instance.SortedLeaderboard != null)
        {
            rankText.text = nameText.text = scoreText.text = "";

            int rank = 1;

            foreach (KeyValuePair<string, int> pair in Leaderboard.Instance.SortedLeaderboard)
            {
                rankText.text += rank.ToString() + "\n";
                nameText.text += pair.Key + "\n";
                scoreText.text += pair.Value + "\n";

                rank++;
            }
        }
    }
}
