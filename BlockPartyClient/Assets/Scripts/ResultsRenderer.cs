using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultsRenderer : MonoBehaviour
{
    Text scoreText, levelText, blocksClearedText, combosText, chainsText;

    // Use this for initialization
    void Start()
    {
        scoreText = GameObject.Find("Score Value").GetComponent<Text>();
        levelText = GameObject.Find("Level Value").GetComponent<Text>();
        blocksClearedText = GameObject.Find("Blocks Cleared Value").GetComponent<Text>();
        combosText = GameObject.Find("Combos Value").GetComponent<Text>();
        chainsText = GameObject.Find("Chains Value").GetComponent<Text>();
    }
	
    // Update is called once per frame
    void Update()
    {
        scoreText.text = StatsTracker.Instance.Score.ToString();
        levelText.text = StatsTracker.Instance.Level.ToString();
        blocksClearedText.text = StatsTracker.Instance.Matches.ToString();
        combosText.text = StatsTracker.Instance.Combos.ToString();
        chainsText.text = StatsTracker.Instance.Chains.ToString();
    }
}
