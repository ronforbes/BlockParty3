using UnityEngine;
using System.Collections;

/// <summary>
/// Represents a block on the game board
/// </summary>
public class Block : MonoBehaviour
{
    public enum BlockState
    {
        Empty,
        Idle,
        Sliding,
        WaitingToFall,
        Falling,
        Matched,
        WaitingToClear,
        Clearing,
        WaitingToEmpty,
    }

    /// <summary>
    /// The position of the block along the horizontal axis
    /// </summary>
    public int X;

    /// <summary>
    /// The position of the block along the vertical axis
    /// </summary>
    public int Y;

    /// <summary>
    /// The state of the block
    /// </summary>
    public BlockState State;

    /// <summary>
    /// The type of the block, used for matching
    /// </summary>
    public int Type;

    /// <summary>
    /// The number of different types used for matching blocks
    /// </summary>
    public const int TypeCount = 6;

    /// <summary>
    /// Initializes the block by setting its state to empty and unmatchable type
    /// </summary>
    void Awake()
    {
        State = BlockState.Empty;
        Type = -1;
    }

    /// <summary>
    /// Creates a block by setting it to the idle state and specified type
    /// </summary>
    /// <param name="type">The type to set the block to</param>
    public void Create(int type)
    {
        State = BlockState.Idle;
        Type = type;
    }
}
