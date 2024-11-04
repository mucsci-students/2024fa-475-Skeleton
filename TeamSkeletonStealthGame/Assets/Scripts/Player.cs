using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PlayerMoves))]

public class Player : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator movement;
    protected PlayerMoves moves;
    protected SpriteRenderer render;
    protected float moveX;
    protected float moveY;
    protected int securityClearance;

    private int hp = 100;
    protected float hitTimer = 0.1f;
    protected bool isAlive = true;
    protected bool isStealth;
    public UnityAction onDeath;
    public Vector3 respawnPoint;



    void Start()
    {
        hp = 100;
        securityClearance = 0;
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Animator>();
        moves = GetComponent<PlayerMoves>();
        respawnPoint = transform.position;
    }

    void Update()
    {
        if (!isAlive) return;
        UpdateHitTimer();
    }



    public void HealDamage(int heal)
    {
        if (isAlive)
            hp = Mathf.Min(hp + heal, 100);
        
    }

    public int getHP()
    {
        return hp;
    }

    public void TakeDamage(int damage)
    {
        
        hp -= damage;
        if (hp <= 0)
                Die();
        render.color = Color.red;
        // Reset color after a short delay
        Invoke(nameof(ResetColor), 0.1f); // Adjust delay if needed
        Vector2 knockbackDirection = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
        float knockbackForce = 5f; // Adjust this value to control the intensity of the knockback
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

           
        
    }

    private void ResetColor()
    {
        // Reset to original color
        render.color = Color.white; // Assuming the original color is white
    }

            
    

    protected void Die()
    {
        Debug.Log("Player has died.");
        rb.velocity = Vector2.zero;

        movement.SetTrigger("Die");
        isAlive = false;
        movement.enabled = false;
        Sprite current=GetComponent<SpriteRenderer>().sprite;
        current = Resources.Load<Sprite>("Dead");
        // Play death sound

        onDeath?.Invoke();
    }


    protected void UpdateHitTimer()
    {
        if (hitTimer < 0.1f)
            hitTimer += Time.deltaTime;
    }


    public void GiveClearance(int securityLevel)
    {
        securityClearance = securityLevel;
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Attacker"))
        {
            Debug.Log("Player is being attacked");
            TakeDamage(10);
        } else if (col.gameObject.CompareTag("Checkpoint")) {
            respawnPoint = transform.position;
        }
    }
}
