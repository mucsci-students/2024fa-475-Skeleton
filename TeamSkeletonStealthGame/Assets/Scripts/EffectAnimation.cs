using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; 
    public Sprite[] frames;
    public bool isenabled = false; 
    public float framesPerSecond = 12f; // Frames per second

    private int currentFrame; 


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentFrame = 0;
    }

    void Update()
    {
        if (isenabled)
        {
        currentFrame = (int)(Time.time * framesPerSecond) % frames.Length;
        spriteRenderer.sprite = frames[currentFrame];
        }
    }


}
