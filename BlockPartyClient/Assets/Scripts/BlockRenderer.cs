using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockRenderer : MonoBehaviour
{
    int x, y;
    Block block;
    SpriteRenderer spriteRenderer;
    public List<Sprite> Sprites;

    // Use this for initialization
    void Awake()
    {
        block = GetComponent<Block>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
	
    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // Update is called once per frame
    void Update()
    {
        switch (block.State)
        {
            case Block.BlockState.Empty:
                spriteRenderer.enabled = false;
                break;
            case Block.BlockState.Idle:
                spriteRenderer.sprite = Sprites [block.Type];
                break;
        }

        transform.position = new Vector3(x, y, 0.0f);
    }
}
