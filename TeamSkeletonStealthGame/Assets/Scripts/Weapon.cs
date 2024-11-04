using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Weapon : MonoBehaviour
{
    public Rigidbody2D wpnBody;
    private Animation weaponAnimation;
    public BoxCollider2D wpnCollider;
    public Sprite wpnSprite;
    public int damage = 1; // Damage the weapon deals
    public float weight = 1.0f; // Weight for player movement
    public bool isProjectile = false; // If the weapon is a projectile
    private SpriteRenderer render;

    void Start()
    {
        wpnBody = GetComponent<Rigidbody2D>();
        weaponAnimation = GetComponent<Animation>();
        wpnBody.gravityScale = 0; // Prevent weapon from falling
        wpnCollider = GetComponent<BoxCollider2D>();
        render = GetComponent<SpriteRenderer>();
        
        if (render != null)
        {
            if (wpnSprite == null)
            {
                wpnSprite = render.sprite;
            }
            render.sprite = wpnSprite; // Assign sprite to renderer
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        
        var targetHealth = col.gameObject.GetComponent<EnemyHealth>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage); // Deal damage
        }
    }

    public void Slash()
    {

        if (!weaponAnimation.IsPlaying("WeaponSlash")) 
        {
            Vector3 currentPosition = transform.localPosition;
            transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);

            weaponAnimation.Play("WeaponSlash"); 
        }
    }

}

