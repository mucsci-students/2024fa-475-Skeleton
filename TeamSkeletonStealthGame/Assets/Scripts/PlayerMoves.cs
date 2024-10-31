using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerMoves : Player
{
    public float normalSpeed = 3.0f;
    public float stealthSpeed = 1.5f;

    private void Start()
    {
        // Initialize Player components
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<Animator>();

        audio_source = GetComponent<AudioSource>();
        onDeath += HandlePlayerDeath;
        onHit += HandlePlayerHit;
    }


    //stop game and go to menu logic or last checkpoint.
    private void HandlePlayerDeath()
    {
        Debug.Log("Game Over! Player has died.");
        Time.timeScale = 0f;

    }

    private void HandlePlayerHit()
    {

    }

    private void Update()
    {
        if (!isAlive) return;
        HandleInput();
    }

    void FixedUpdate()
    {
        if (!isAlive) return;
        HandleMovement();
    }

    private void HandleInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonDown(0))
            Punch();

        if (Input.GetKeyDown(KeyCode.Space))
            Jumpkick();

        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(20);

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            ToggleStealth();
        
    }

    private void HandleMovement()
    {
        float speed = isStealth ? stealthSpeed : normalSpeed;
        Vector2 movementVector = new Vector2(moveX * speed, moveY * speed);

        rb.velocity = movementVector;

        bool isMoving = movementVector.magnitude > 0.01f;
        movement.SetBool("isMoving", isMoving);

        if (isMoving && moveX != 0) {
            transform.localScale = new Vector3(Mathf.Sign(moveX) * 0.8f, 0.8f, 0.8f);
            if (moveX > 0) {
                this.GetComponent<FieldOfView>().fovRotation = 85f;
                this.GetComponent<FieldOfView>().DrawFieldofView();
            } else {
                this.GetComponent<FieldOfView>().fovRotation = 265f;
                this.GetComponent<FieldOfView>().DrawFieldofView();
            }
        }
    }

    private void Jumpkick()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(Vector2.up * 2, ForceMode2D.Impulse);
            movement.SetTrigger("Jumpkick");
        }
    }

    private void Punch()
    {
        movement.SetTrigger("Punch");
    }

    private void ToggleStealth()
    {
        isStealth = !isStealth;
        movement.SetBool("isStealthy", isStealth);
        Debug.Log("Stealth mode: " + (isStealth ? "On" : "Off"));
    }
}
