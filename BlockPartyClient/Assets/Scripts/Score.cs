using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int GameScore;
    const int chainMultiplier = 100;
    Text scoreText;

    // Use this for initialization
    void Start()
    {
        scoreText = GameObject.Find("Score Value").GetComponent<Text>();
    }
    
    public void ReportChain(Chain chain)
    {
        GameScore += chain.Magnitude * chainMultiplier;
        scoreText.text = GameScore.ToString();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}