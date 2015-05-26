using UnityEngine;
using System.Collections;

/// <summary>
/// Component that enables blocks to track state for chaining matches
/// </summary>
public class BlockChaining : MonoBehaviour
{
    /// <summary>
    /// Tracks whether the block was just emptied, causing blocks above it to be eligible to participate in a chain
    /// </summary>
    public bool JustEmptied;

    /// <summary>
    /// Tracks whether a block is eligible to participate in a chain
    /// </summary>
    public bool ChainEligible;
}
