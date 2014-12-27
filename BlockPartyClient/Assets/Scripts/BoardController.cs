using UnityEngine;
using System.Collections;

public class BoardController : MonoBehaviour
{
    Board board;
    Block selectedBlock;

    // Use this for initialization
    void Awake()
    {
        board = GetComponent<Board>();
    }
	
    // Update is called once per frame
    void Update()
    {
        // If the mouse button is clicked, find the block it's over, and select it
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.name == "Block(Clone)")
            {
                Block block = hit.collider.gameObject.GetComponent<Block>();
                if (block.State == Block.BlockState.Idle && block.Y > 0)
                {
                    selectedBlock = block;
                }
            }
        }

        // If the mouse button is released, clear the selected block
        if (Input.GetMouseButtonUp(0))
        {
            selectedBlock = null;
        }

        // If a block has been selected, and the mouse has been dragged beyond its left or right edge,
        // determine the left and right blocks to be slid
        if (selectedBlock != null)
        {
            float leftEdge = (float)selectedBlock.X - selectedBlock.transform.localScale.x / 2;
            float rightEdge = (float)selectedBlock.X + selectedBlock.transform.localScale.x / 2;

            Block leftBlock = null, rightBlock = null;

            Vector3 mousePosition = Input.mousePosition;

            if (Camera.main.ScreenToWorldPoint(mousePosition).x < leftEdge &&
                selectedBlock.State == Block.BlockState.Idle &&
                selectedBlock.X - 1 >= 0 &&
                (board.Blocks [selectedBlock.X - 1, selectedBlock.Y].State == Block.BlockState.Idle ||
                board.Blocks [selectedBlock.X - 1, selectedBlock.Y].State == Block.BlockState.Empty))
            {
                leftBlock = board.Blocks [selectedBlock.X - 1, selectedBlock.Y];
                rightBlock = selectedBlock;

                selectedBlock = leftBlock;
            }

            if (Camera.main.ScreenToWorldPoint(mousePosition).x > rightEdge &&
                selectedBlock.State == Block.BlockState.Idle &&
                selectedBlock.X + 1 < Board.Columns &&
                (board.Blocks [selectedBlock.X + 1, selectedBlock.Y].State == Block.BlockState.Idle ||
                board.Blocks [selectedBlock.X + 1, selectedBlock.Y].State == Block.BlockState.Empty))
            {
                leftBlock = selectedBlock;
                rightBlock = board.Blocks [selectedBlock.X + 1, selectedBlock.Y];

                selectedBlock = rightBlock;
            }

            if (leftBlock != null && rightBlock != null)
            {
                SetupSlide(leftBlock, BlockSlider.SlideDirection.Right);
                SetupSlide(rightBlock, BlockSlider.SlideDirection.Left);

                leftBlock.GetComponent<BlockSlider>().Slide(BlockSlider.SlideDirection.Right);
                rightBlock.GetComponent<BlockSlider>().Slide(BlockSlider.SlideDirection.Left);
            }
        }
    }

    void SetupSlide(Block block, BlockSlider.SlideDirection direction)
    {
        // Save off the state of the block that this one will swap with
        Block targetBlock = null;
        if (direction == BlockSlider.SlideDirection.Left)
        {
            targetBlock = board.Blocks [block.X - 1, block.Y];
        }
        
        if (direction == BlockSlider.SlideDirection.Right)
        {
            targetBlock = board.Blocks [block.X + 1, block.Y];
        }

        BlockSlider slider = block.GetComponent<BlockSlider>();

        slider.TargetState = targetBlock.State;
        slider.TargetType = targetBlock.Type;
    }
}
