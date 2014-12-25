using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    public enum BlockState
    {
        Empty,
        Idle,
    }

    public BlockState State;
    BlockRenderer blockRenderer;
    public int Type;
    public const int TypeCount = 6;

    // Use this for initialization
    void Awake()
    {
        State = BlockState.Empty;
        Type = -1;
        blockRenderer = GetComponent<BlockRenderer>();
    }
	
    public void InitializeRenderer(int x, int y)
    {
        blockRenderer.Initialize(x, y);
    }

    public void Create(int type)
    {
        State = BlockState.Idle;
        Type = type;
    }

    // Update is called once per frame
    void Update()
    {
	
    }
}
