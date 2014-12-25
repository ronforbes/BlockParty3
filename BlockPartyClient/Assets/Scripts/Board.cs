using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    Block[,] blocks;
    public Block BlockPrefab;
    const int columns = 6;
    const int rows = 10;
    List<int> lastNewRowBlockTypes = new List<int>(columns);
    List<int> secondToLastNewRowBlockTypes = new List<int>(columns);
    int lastNewBlockType = 0, secondToLastNewBlockType = 0;

    // Use this for initialization
    void Awake()
    {
        // initialize the blocks
        blocks = new Block[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                blocks [x, y] = Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity) as Block;
                blocks [x, y].transform.parent = transform;
                blocks [x, y].InitializeRenderer(x, y);
            }
        }

        // initialize the previous new rows' block types 
        for (int x = 0; x < columns; x++)
        {
            lastNewRowBlockTypes.Add(0);
            secondToLastNewRowBlockTypes.Add(0);
        }

        // populate the board w/ blocks
        int shortColumn = Random.Range(0, columns);
        
        for (int x = columns - 1; x >= 0; x--)
        {
            int height = (x == shortColumn ? 2 : 7) + Random.Range(0, 2);
            
            for (int y = height - 1; y >= 1; y--)
            {
                int type;

                // choose a random block type that doesn't match the blocks above or to the right of it
                do
                {
                    type = Random.Range(0, Block.TypeCount);
                    
                    if (blocks [x, y + 1].State != Block.BlockState.Empty &&
                        blocks [x, y + 1].Type == type)
                        continue;
                    
                    if (x == columns - 1)
                        break;
                    
                    if (blocks [x + 1, y].State != Block.BlockState.Empty &&
                        blocks [x + 1, y].Type == type)
                        continue;
                    
                    break;
                } while (true);
                
                // setup new row creation state
                if (y == 2)
                    secondToLastNewRowBlockTypes [x] = type;
                
                if (y == 1)
                    lastNewRowBlockTypes [x] = type;
                
                // create the block
                blocks [x, y].Create(type);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
	
    }
}
