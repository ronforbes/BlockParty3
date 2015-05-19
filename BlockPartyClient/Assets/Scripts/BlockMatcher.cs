using UnityEngine;
using System.Collections;

public class BlockMatcher : MonoBehaviour
{
    Block block;
    BlockClearer clearer;
    BlockEmptier emptier;
    float elapsed;
    const float duration = 1.0f;

    // Use this for initialization
    void Awake()
    {
        block = GetComponent<Block>();
        clearer = GetComponent<BlockClearer>();
        emptier = GetComponent<BlockEmptier>();
    }
	
    public void Match(int matchedBlockCount, int delayCounter, Chain chain)
    {
        block.State = Block.BlockState.Matched;

        GetComponent<BlockChaining>().BeginChainInvolvement(chain);

        elapsed = 0.0f;

        clearer.DelayDuration = (matchedBlockCount - delayCounter) * BlockClearer.DelayInterval;
        emptier.DelayDuration = delayCounter * BlockEmptier.DelayInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (block.State == Block.BlockState.Matched)
        {
            elapsed += Time.deltaTime;

            if (elapsed >= duration)
            {
                GetComponent<BlockChaining>().Chain.DecrementInvolvement();

                clearer.Clear();
            }
        }
    }
}
