using UnityEngine;
using System.Collections;

public class BoardGravity : MonoBehaviour
{
    Board board;
    MatchDetector matchDetector;

    // Use this for initialization
    void Awake()
    {
        board = GetComponent<Board>();
        matchDetector = GameObject.Find("Game").GetComponent<MatchDetector>();
    }
	
    // Update is called once per frame
    void Update()
    {
        for (int x = 0; x < Board.Columns; x++)
        {
            bool emptyBlockDetected = false;

            for (int y = 1; y < Board.Rows; y++)
            {
                if (board.Blocks [x, y].State == Block.BlockState.Empty)
                {
                    emptyBlockDetected = true;
                }

                if (board.Blocks [x, y].State == Block.BlockState.Idle && emptyBlockDetected)
                {
                    board.Blocks [x, y].GetComponent<BlockFaller>().Target = board.Blocks [x, y - 1];
                    board.Blocks [x, y].GetComponent<BlockFaller>().Fall(board.Blocks [x, y - 1].GetComponent<BlockChaining>().Chain);
                }

                if (board.Blocks [x, y].GetComponent<BlockFaller>().JustFell)
                {
                    if (y - 1 >= 1 && board.Blocks [x, y - 1].State == Block.BlockState.Empty || board.Blocks [x, y - 1].State == Block.BlockState.Falling)
                    {
                        board.Blocks [x, y].GetComponent<BlockFaller>().Target = board.Blocks [x, y - 1];
                        board.Blocks [x, y].GetComponent<BlockFaller>().ContinueFalling();
                    } else
                    {
                        board.Blocks [x, y].State = Block.BlockState.Idle;

                        matchDetector.RequestMatchDetection(board.Blocks [x, y]);
                    }

                    board.Blocks [x, y].GetComponent<BlockFaller>().JustFell = false;
                }
            }
        }
    }
}
