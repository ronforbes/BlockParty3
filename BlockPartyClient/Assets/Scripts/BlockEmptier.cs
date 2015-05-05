using UnityEngine;
using System.Collections;

public class BlockEmptier : MonoBehaviour
{
    Block block;
    float delayElapsed;
    public const float DelayInterval = 1.0f; //0.25f;
    public float DelayDuration;

    // Use this for initialization
    void Awake()
    {
        block = GetComponent<Block>();
    }
	
    public void Empty()
    {
        block.State = Block.BlockState.WaitingToEmpty;

        delayElapsed = 0.0f;
    }

    void FinishEmptying()
    {
        block.State = Block.BlockState.Empty;
        block.Type = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (block.State == Block.BlockState.WaitingToEmpty)
        {
            delayElapsed += Time.deltaTime;

            if (delayElapsed >= DelayDuration)
            {
                FinishEmptying();
            }
        }
    }
}
