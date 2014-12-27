using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    public enum BlockState
    {
        Empty,
        Idle,
        Sliding,
    }

    public int X, Y;
    public BlockState State;
    public int Type;
    public const int TypeCount = 6;

    // Use this for initialization
    void Awake()
    {
        State = BlockState.Empty;
        Type = -1;
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
