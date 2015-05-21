using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchDetection
{
    public Block Block;

    public MatchDetection(Block block)
    {
        Block = block;
    }
}

public class MatchDetector : MonoBehaviour
{
    List<MatchDetection> matchDetections;
    Board board;
    //ChainDetector chainDetector;
    Stats stats;
    public const int MinimumMatchLength = 3;

    // Use this for initialization
    void Awake()
    {
        matchDetections = new List<MatchDetection>();
        board = GetComponent<Board>();
        //chainDetector = GetComponent<ChainDetector>();
        stats = GetComponent<Stats>();
    }
	
    public void RequestMatchDetection(Block block)
    {
        matchDetections.Add(new MatchDetection(block));
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
                DetectMatch(detection.Block);
            }
        }
    }

    void DetectMatch(Block block)
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
            return;
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
                board.Blocks [killX, block.Y].GetComponent<BlockMatcher>().Match(matchedBlockCount, delayCounter);
                delayCounter--;
                if (board.Blocks [killX, block.Y].GetComponent<BlockChaining>().ChainEligible)
                {
                    //incrementChain = true;
                }
            }
        }
        
        if (verticalMatch)
        {
            // kill the pattern's blocks
            for (int killY = top - 1; killY >= bottom; killY--)
            {
                board.Blocks [block.X, killY].GetComponent<BlockMatcher>().Match(matchedBlockCount, delayCounter);
                delayCounter--;
                if (board.Blocks [block.X, killY].GetComponent<BlockChaining>().ChainEligible)
                {
                    //incrementChain = true;
                }
            }
        }

        // handle combos
        if (matchedBlockCount > 3)
        {
            stats.ScoreCombo(matchedBlockCount);
        }

        /*if(incrementChain)
        {
            chainCount++;
            Stats.ScoreChain(chainCount);
        }*/
    }
}
