using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchDetection
{
    public Block Block;
    public Chain Chain;

    public MatchDetection(Block block, Chain chain)
    {
        Block = block;
        Chain = chain;
    }
}

public class MatchDetector : MonoBehaviour
{
    List<MatchDetection> matchDetections;
    Board board;
    ChainDetector chainDetector;
    public const int MinimumMatchLength = 3;

    // Use this for initialization
    void Awake()
    {
        matchDetections = new List<MatchDetection>();
        board = GetComponent<Board>();
        chainDetector = GetComponent<ChainDetector>();
    }
	
    public void RequestMatchDetection(Block block, Chain chain = null)
    {
        matchDetections.Add(new MatchDetection(block, chain));
    }

    // Update is called once per frame
    void Update()
    {
        while (matchDetections.Count > 0)
        {
            MatchDetection detection = matchDetections [0];
            matchDetections.Remove(detection);

            // ensure that the block is still idle
            if (detection.Block.State == Block.BlockState.Idle)
            {
                DetectMatch(detection.Block, detection.Block.GetComponent<BlockChaining>().Chain != null ? detection.Block.GetComponent<BlockChaining>().Chain : detection.Chain);
            }
        }
    }

    void DetectMatch(Block block, Chain chain)
    {
        // look in four directions for matching blocks   
        int left = block.X;
        while (left > 0)
        {
            if (board.Blocks [left - 1, block.Y].State != Block.BlockState.Idle)
                break;
            if (board.Blocks [left - 1, block.Y].Type != block.Type)
                break;
            left--;
        }
        
        int right = block.X + 1;
        while (right < Board.Columns)
        {
            if (board.Blocks [right, block.Y].State != Block.BlockState.Idle)
                break;
            if (board.Blocks [right, block.Y].Type != block.Type)
                break;
            right++;
        }
        
        int bottom = block.Y;
        while (bottom > 1)
        {
            if (board.Blocks [block.X, bottom - 1].State != Block.BlockState.Idle)
                break;
            if (board.Blocks [block.X, bottom - 1].Type != block.Type)
                break;
            bottom--;
        }
        
        int top = block.Y + 1;
        while (top < Board.Rows)
        {
            if (board.Blocks [block.X, top].State != Block.BlockState.Idle)
                break;
            if (board.Blocks [block.X, top].Type != block.Type)
                break;
            top++;
        }
        
        int width = right - left;
        int height = top - bottom;
        int matchedBlockCount = 0;
        bool horizontalMatch = false;
        bool verticalMatch = false;
        
        if (width >= MinimumMatchLength)
        {
            horizontalMatch = true;
            matchedBlockCount += width;
        }
        
        if (height >= MinimumMatchLength)
        {
            verticalMatch = true;
            matchedBlockCount += height;
        }
        
        if (!horizontalMatch && !verticalMatch)
        {
            block.GetComponent<BlockChaining>().EndChainInvolvement(chain);
            return;
        }
        
        if (chain == null)
        {
            chain = chainDetector.CreateChain();
        }
        
        // if pattern matches both directions
        if (horizontalMatch && verticalMatch)
            matchedBlockCount--;

        int delayCounter = matchedBlockCount;

        // kill the pattern's blocks and look for touching garbage
        
        if (horizontalMatch)
        {
            // kill the pattern's blocks
            for (int killX = left; killX < right; killX++)
            {
                //if (killX != block.X)
                //{
                board.Blocks [killX, block.Y].GetComponent<BlockMatcher>().Match(matchedBlockCount, delayCounter, chain);
                delayCounter--;
                //}
            }
        }
        
        if (verticalMatch)
        {
            // kill the pattern's blocks
            for (int killY = top - 1; killY >= bottom; killY--)
            {
                //if (killY != block.Y)
                //{
                board.Blocks [block.X, killY].GetComponent<BlockMatcher>().Match(matchedBlockCount, delayCounter, chain);
                delayCounter--;
                //}
            }
        }
        
        chain.ReportMatch(matchedBlockCount, block);
    }
}
