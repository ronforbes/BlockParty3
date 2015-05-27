using UnityEngine;
using System.Collections;

/// <summary>
/// Component enabling blocks to be cleared from the board
/// </summary>
public class BlockClearer : MonoBehaviour
{
    /// <summary>
    /// The block to be cleared.
    /// </summary>
    Block block;

    /// <summary>
    /// The block's emptying component
    /// </summary>
    BlockEmptier emptier;   

    /// <summary>
    /// The amount of time that the block has been waiting to clear
    /// </summary>
    float delayElapsed;

    /// <summary>
    /// The interval of time that blocks are delayed from clearing
    /// </summary>
    public const float DelayInterval = 0.25f;

    /// <summary>
    /// The duration of the delay before blocks are cleared
    /// </summary>
    public float DelayDuration;

    /// <summary>
    /// The amount of time that a block has been clearing
    /// </summary>
    public float Elapsed;

    /// <summary>
    /// The duration of time taken to clear a block
    /// </summary>
    public const float Duration = 0.25f;

    /// <summary>
    /// Initializes the clearer by getting the associated block, emptier, and stats tracker
    /// </summary>
    void Awake()
    {
        block = GetComponent<Block>();
        emptier = GetComponent<BlockEmptier>();
    }
	
    /// <summary>
    /// Starts clearing the block from the board
    /// </summary>
    public void Clear()
    {
        block.State = Block.BlockState.WaitingToClear;

        delayElapsed = 0.0f;
    }

    /// <summary>
    /// Waits for the clear delay and then clears the block from the board
    /// </summary>
    void Update()
    {
        if (block.State == Block.BlockState.WaitingToClear)
        {
            delayElapsed += Time.deltaTime;

            if (delayElapsed >= DelayDuration)
            {
                block.State = Block.BlockState.Clearing;
                
                Elapsed = 0.0f;
                
                StatsTracker.Instance.ScoreMatch();
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
