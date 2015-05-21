using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainDetector : MonoBehaviour
{
    //int chainCount = 0;
    Board board;

    void Awake()
    {
        board = GetComponent<Board>();
    }
    
    void Update()
    {
        //bool stopChain = true;

        for (int x = 0; x < Board.Columns; x++)
        {
            for (int y = Board.Rows - 1; y >= 0; y--)
            {
                if (board.Blocks [x, y].GetComponent<BlockChaining>().JustEmptied)
                {
                    //for(int chainEligibleRow = y + 1; chainEligibleRow >= 0)
                }
            }
        }
    }
}