using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioClip))]
[RequireComponent(typeof(AudioClip))]
[RequireComponent(typeof(PlayerMoves))]


public class Player : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator movement;
    protected PlayerMoves moves;
    protected FieldOfView FOV;
    protected float moveX;
    protected float moveY;
    protected int hp;
    protected float hitTimer = 0.1f;
    protected bool isAlive = true;
    protected bool isStealth;
    public UnityAction onDeath;
    public UnityAction onHit;
    protected AudioSource audio_source;
    public AudioClip hit_sound; // player gets hit
    public AudioClip death_sound; // player death
    public Vector3 respawnPoint;



    void Start()
    {
        hp = 100;
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Animator>();
        moves = GetComponent<PlayerMoves>();
        audio_source = GetComponent<AudioSource>();
        hit_sound = Resources.Load<AudioClip>("Playerhit");
        death_sound = Resources.Load<AudioClip>("Death");
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

    public void TakeDamage(int damage)
    {
        if (isAlive && hitTimer >= 0.1f)
        {
            hp -= damage;
            hitTimer = 0f;
            // Play hit sound
            audio_source?.PlayOneShot(hit_sound);

            Vector2 knockbackDirection = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
            float knockbackForce = 5f; // Adjust this value to control the intensity of the knockback
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            if (hp <= 0)
                Die();
            else
                onHit?.Invoke();
        }
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
        audio_source?.PlayOneShot(death_sound);

        onDeath?.Invoke();
    }


    protected void UpdateHitTimer()
    {
        if (hitTimer < 0.1f)
            hitTimer += Time.deltaTime;
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
