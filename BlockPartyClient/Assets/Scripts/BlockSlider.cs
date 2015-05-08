using UnityEngine;
using System.Collections;

public class BlockSlider : MonoBehaviour
{
    public enum SlideDirection
    {
        Left,
        Right,
        None
    }

    Block block;
    Board board;
    MatchDetector matchDetector;
    public SlideDirection Direction;
    public Block.BlockState TargetState;
    public int TargetType;
    public float Elapsed;
    public const float Duration = 0.1f;

    // Use this for initialization
    void Awake()
    {
        block = GetComponent<Block>();
        board = GameObject.Find("Game").GetComponent<Board>();
        matchDetector = GameObject.Find("Game").GetComponent<MatchDetector>();
    }
	
    public void Slide(SlideDirection direction)
    {
        block.State = Block.BlockState.Sliding;

        // Reset the sliding timer
        Elapsed = 0.0f;

        Direction = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (block.State == Block.BlockState.Sliding)
        {
            Elapsed += Time.deltaTime;

            if (Elapsed >= Duration)
            {
                block.State = TargetState;
                block.Type = TargetType;

                Direction = SlideDirection.None;

                if (block.State == Block.BlockState.Idle)
                {
                    matchDetector.RequestMatchDetection(block);
                }
            }
        }
    }
}
