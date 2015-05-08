using UnityEngine;
using System.Collections;

public class BoardRaiser : MonoBehaviour
{
    Board board;
    float raiseRate = 1.0f;
    public float Elapsed;
    public const float Duration = 10.0f;

    // Use this for initialization
    void Awake()
    {
        board = GetComponent<Board>();
    }
	
    // Update is called once per frame
    void Update()
    {
        float rate = raiseRate;

        for (int x = 0; x < Board.Columns; x++)
        {
            for (int y = 1; y < Board.Rows; y++)
            {
                if (board.Blocks [x, y].State != Block.BlockState.Empty &&
                    board.Blocks [x, y].State != Block.BlockState.Idle &&
                    board.Blocks [x, y].State != Block.BlockState.Sliding)
                {
                    rate = 0.0f;
                }

                if (y == Board.Rows - 1)
                {
                    if (board.Blocks [x, y].State != Block.BlockState.Empty)
                    {
                        rate = 0.0f;
                    }
                }
            }
        }

        Elapsed += Time.deltaTime * rate;

        if (Elapsed >= Duration)
        {
            Elapsed = 0.0f;

            for (int x = 0; x < Board.Columns; x++)
            {
                for (int y = Board.Rows - 1; y >= 1; y--)
                {
                    board.Blocks [x, y].State = board.Blocks [x, y - 1].State;
                    board.Blocks [x, y].Type = board.Blocks [x, y - 1].Type;

                    if (board.Blocks [x, y].State == Block.BlockState.Sliding)
                    {
                        BlockSlider slider = board.Blocks [x, y].GetComponent<BlockSlider>();
                        BlockSlider sliderBelow = board.Blocks [x, y - 1].GetComponent<BlockSlider>();

                        slider.Direction = sliderBelow.Direction;
                        slider.TargetState = sliderBelow.TargetState;
                        slider.TargetType = sliderBelow.TargetType;
                        slider.Elapsed = sliderBelow.Elapsed;
                    }
                }
            }

            board.CreateNewRow();
        }
    }
}
