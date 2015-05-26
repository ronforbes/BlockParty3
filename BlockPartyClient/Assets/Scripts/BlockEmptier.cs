using UnityEngine;
using System.Collections;

/// <summary>
/// Component enabling blocks to be emptied from the board
/// </summary>
public class BlockEmptier : MonoBehaviour
{
    /// <summary>
    /// The block to be emptied
    /// </summary>
    Block block;

    /// <summary>
    /// The associated block chaining component
    /// </summary>
    BlockChaining chaining;

    /// <summary>
    /// The amount of time that emptying has been delayed
    /// </summary>
    float delayElapsed;

    /// <summary>
    /// The interval of time that blocks are delayed from being emptied
    /// </summary>
    public const float DelayInterval = 0.25f;

    /// <summary>
    /// The duration of the delay before emptying the block
    /// </summary>
    public float DelayDuration;

    /// <summary>
    /// Initializes the block emptier by getting the associated block and chaining component
    /// </summary>
    void Awake()
    {
        block = GetComponent<Block>();
        chaining = GetComponent<BlockChaining>();
    }
	
    /// <summary>
    /// Empties the block by setting its state and resetting the delay timer
    /// </summary>
    public void Empty()
    {
        block.State = Block.BlockState.WaitingToEmpty;

        delayElapsed = 0.0f;
    }

    /// <summary>
    /// Waits for the delay timer and then empties the block
    /// </summary>
    void Update()
    {
        if (block.State == Block.BlockState.WaitingToEmpty)
        {
            delayElapsed += Time.deltaTime;

            if (delayElapsed >= DelayDuration)
            {
                block.State = Block.BlockState.Empty;
                block.Type = -1;
                chaining.JustEmptied = true;
            }
        }
    }
}
