using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Stats : MonoBehaviour
{
    public int Score = 0;
    public int Level = 1;
    public float TimeRemaining = 120.0f;
    public int Matches = 0;
    public int Combos = 0;
    public Dictionary<int, int> ComboBreakdown = new Dictionary<int, int>();
    public int Chains = 0;
    public Dictionary<int, int> ChainBreakdown = new Dictionary<int, int>();

    const int matchValue = 1;
    const int comboValue = 10;
    const int chainValue = 100;

    Text scoreText;

    // Use this for initialization
    void Start()
    {
        for (int comboLength = 4; comboLength < Board.Rows * Board.Columns; comboLength++)
        {
            ComboBreakdown.Add(comboLength, 0);
        }

        for (int chainLength = 2; chainLength < Board.Rows * Board.Columns / 3; chainLength++)
        {
            ChainBreakdown.Add(chainLength, 0);
        }

        scoreText = GameObject.Find("Score Value").GetComponent<Text>();
    }

    public void ScoreMatch()
    {
        Score += matchValue;
        Matches++;
    }

    public void ScoreCombo(int comboLength)
    {
        Score += matchValue * comboValue;
        Combos++;

        if (!ComboBreakdown.ContainsKey(comboLength))
        {
            ComboBreakdown.Add(comboLength, 0);
        }
        ComboBreakdown [comboLength]++;
    }

    public void ScoreChain(int chainLength)
    {
        Score += chainLength * chainValue;
        Chains++;

        if (!ChainBreakdown.ContainsKey(chainLength))
        {
            ChainBreakdown.Add(chainLength, 0);
        }
        ChainBreakdown [chainLength]++;
    }
    
    // Update is called once per frame
    void Update()
    {
        TimeRemaining -= Time.deltaTime;

        Level = (int)Mathf.Clamp(Score / 1000, 1, 99);

        scoreText.text = Score.ToString();
    }
}