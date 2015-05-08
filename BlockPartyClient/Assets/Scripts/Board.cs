using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    public Block[,] Blocks;
    public Block BlockPrefab;
    public const int Columns = 6;
    public const int Rows = 11;
    List<int> lastNewRowBlockTypes = new List<int>(Columns);
    List<int> secondToLastNewRowBlockTypes = new List<int>(Columns);
    int lastNewBlockType = 0, secondToLastNewBlockType = 0;

    // Use this for initialization
    void Awake()
    {
        // initialize the blocks
        Blocks = new Block[Columns, Rows];
        for (int x = 0; x < Columns; x++)
        {
            for (int y = 0; y < Rows; y++)
            {
                Blocks [x, y] = Instantiate(BlockPrefab, Vector3.zero, Quaternion.identity) as Block;
                Blocks [x, y].transform.parent = transform;
                Blocks [x, y].X = x;
                Blocks [x, y].Y = y;
            }
        }

        // initialize the previous new rows' block types 
        for (int x = 0; x < Columns; x++)
        {
            lastNewRowBlockTypes.Add(0);
            secondToLastNewRowBlockTypes.Add(0);
        }

        // populate the board w/ blocks
        int shortColumn = Random.Range(0, Columns);
        
        for (int x = Columns - 1; x >= 0; x--)
        {
            int height = (x == shortColumn ? 2 : 7) + Random.Range(0, 2);
            
            for (int y = height - 1; y >= 1; y--)
            {
                int type;

                // choose a random block type that doesn't match the blocks above or to the right of it
                do
                {
                    type = Random.Range(0, Block.TypeCount);
                    
                    if (Blocks [x, y + 1].State != Block.BlockState.Empty &&
                        Blocks [x, y + 1].Type == type)
                        continue;
                    
                    if (x == Columns - 1)
                        break;
                    
                    if (Blocks [x + 1, y].State != Block.BlockState.Empty &&
                        Blocks [x + 1, y].Type == type)
                        continue;
                    
                    break;
                } while (true);
                
                // setup new row creation state
                if (y == 2)
                    secondToLastNewRowBlockTypes [x] = type;
                
                if (y == 1)
                    lastNewRowBlockTypes [x] = type;
                
                // create the block
                Blocks [x, y].Create(type);
            }
        }

        CreateNewRow();
    }

    public void CreateNewRow()
    {
        for (int x = 0; x < Columns; x++)
        {
            int type = 0;

            do
            {
                type = Random.Range(0, Block.TypeCount);
            } while((type == lastNewBlockType && lastNewBlockType == secondToLastNewBlockType) ||
                    (type == lastNewRowBlockTypes[x] && type == secondToLastNewRowBlockTypes[x]));

            secondToLastNewRowBlockTypes [x] = lastNewRowBlockTypes [x];
            lastNewRowBlockTypes [x] = type;

            secondToLastNewBlockType = lastNewBlockType;
            lastNewBlockType = type;

            Blocks [x, 0].Create(type);
        }
    }

    // Update is called once per frame
    void Update()
    {
	
    }
}
