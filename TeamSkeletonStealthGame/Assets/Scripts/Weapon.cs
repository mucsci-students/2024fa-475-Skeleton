using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Weapon : MonoBehaviour
{
    public Rigidbody2D wpnBody;
    public int damage = 1; // Damage the weapon deals
    public int ammo = 0; // Weight for player movement
    private SpriteRenderer render;

    void Start()
    {
        wpnBody = GetComponent<Rigidbody2D>();
        wpnBody.gravityScale = 0; // Prevent weapon from falling
        render = GetComponent<SpriteRenderer>();
    
    }

    

    

}

