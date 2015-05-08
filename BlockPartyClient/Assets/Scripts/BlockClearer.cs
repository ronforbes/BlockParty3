using UnityEngine;
using System.Collections;

public class BlockClearer : MonoBehaviour
{
    Block block;
    BlockEmptier emptier;
    float delayElapsed;
    public const float DelayInterval = 0.25f;
    public float DelayDuration;
    public float Elapsed;
    public const float Duration = 0.25f;

    // Use this for initialization
    void Awake()
    {
        block = GetComponent<Block>();
        emptier = GetComponent<BlockEmptier>();
    }
	
    public void Clear()
    {
        block.State = Block.BlockState.WaitingToClear;

        delayElapsed = 0.0f;
    }

    void FinishClearing()
    {
        block.State = Block.BlockState.Clearing;

        Elapsed = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (block.State == Block.BlockState.WaitingToClear)
        {
            delayElapsed += Time.deltaTime;

            if (delayElapsed >= DelayDuration)
            {
                FinishClearing();
            }
        }

        if (block.State == Block.BlockState.Clearing)
        {
            Elapsed += Time.deltaTime;

            if (Elapsed >= Duration)
            {
                emptier.Empty();
            }
        }
    }
}
