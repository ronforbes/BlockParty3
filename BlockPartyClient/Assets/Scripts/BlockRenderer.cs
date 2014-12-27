using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockRenderer : MonoBehaviour
{
    Block block;
    BlockSlider slider;
    SpriteRenderer spriteRenderer;
    public List<Sprite> Sprites;

    // Use this for initialization
    void Awake()
    {
        block = GetComponent<Block>();
        slider = GetComponent<BlockSlider>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (block.State)
        {
            case Block.BlockState.Empty:
                transform.position = new Vector3(block.X, block.Y, 0.0f);

                spriteRenderer.enabled = false;
                break;
            
            case Block.BlockState.Idle:
                transform.position = new Vector3(block.X, block.Y, 0.0f);
            
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = Sprites [block.Type];
                break;

            case Block.BlockState.Sliding:
                float destination = 0.0f;
                if (slider.Direction == BlockSlider.SlideDirection.Left)
                {
                    destination = -transform.localScale.x;
                }

                if (slider.Direction == BlockSlider.SlideDirection.Right)
                {
                    destination = transform.localScale.x;
                }

                float timePercentage = slider.Elapsed / BlockSlider.Duration;
                transform.position = Vector3.Lerp(new Vector3(block.X, block.Y, 0.0f), new Vector3(block.X + destination, block.Y, 0.0f), timePercentage);

                if (block.Type == -1)
                {
                    spriteRenderer.enabled = false;
                } else
                {
                    spriteRenderer.enabled = true;
                    spriteRenderer.sprite = Sprites [block.Type];
                }
                break;
        }


    }
}
