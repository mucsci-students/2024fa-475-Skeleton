using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UIElements;

public class PlayerMoves : Player
{
    [SerializeField]
    public float normalSpeed = 3.0f;
    
    public float stealthSpeed = 1.5f;
    public AudioSource punchSound;
    public AudioSource[] footsteps;
    public AudioSource gunshot;
    private FieldOfView FOV;
    private bool isMoving;
    public  Collider2D punchCollider; 
    float footstepTimer = 0f;
    private float footstepInterval = .33f; // third of a second between each footstep sound
    private bool isMovingFastEnough = false; // Whether player is moving fast enough for footstep sounds


    private void Start()
    {
        // Initialize Player components
        rb = GetComponent<Rigidbody2D>();
        punchCollider = GetComponent<Collider2D>();
        punchCollider.enabled = false; 
        render = GetComponent<SpriteRenderer>();
        movement = GetComponent<Animator>();
        FOV = GetComponent<FieldOfView>();
        onDeath += HandlePlayerDeath;
    }

private void HandlePlayerDeath()
{
    Debug.Log("Game Over! Player has died.");
    
    GameObject gameover = GameObject.Find("GameOver");
    if (gameover != null)
    {
        gameover.SetActive(true); // Make UI visible
    }

    StartCoroutine(GameOverStop());
}

private IEnumerator GameOverStop()
{
    Time.timeScale = 1f; 

    yield return new WaitForSecondsRealtime(3f);

    Time.timeScale = 0f; 
}


    private void Update()
    {
        if (!isAlive) return;
        PISSCallReturnScript inPissCall = FindObjectOfType<PISSCallReturnScript>();
        if(inPissCall==null)
                {HandleInput();HandleMovement();}
        
                // Update footstep timer and play sound if moving fast enough
        if (isMovingFastEnough)
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                playStepSound();
                footstepTimer = 0f; // Reset the timer
            }
        }
    }


    private void HandleInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonDown(0)){
            Punch();
            punchSound.Play();
}
        if (Input.GetKeyDown(KeyCode.Space))
            Jumpkick();

        if (Input.GetKeyDown(KeyCode.K))
            TakeDamage(20);

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
            ToggleStealth();
        
    }
private float maxTiltAngle = 10f; // Maximum tilt angle for up/down movement

private void HandleMovement()
{
    float speed = isStealth ? stealthSpeed : normalSpeed;
    Vector2 movementVector = new Vector2(moveX * speed, moveY * speed);

    rb.velocity = movementVector.normalized * speed;

    isMoving = movementVector.magnitude > 0.01f;
    movement.SetBool("isMoving", isMoving);

    if (isMoving)
    {

        render.flipX = moveX < 0;

        // Check if the player is moving fast enough (above a speed threshold)
        isMovingFastEnough = movementVector.magnitude > 0.75f;

       /* // Rotate sprite based on vertical movement
        float targetTilt = moveY * maxTiltAngle;
        float smoothTilt = Mathf.LerpAngle(transform.eulerAngles.z, targetTilt, Time.deltaTime * 5f);
        transform.eulerAngles = new Vector3(0, 0, smoothTilt);*/
    }
    else
    {
        // Reset rotation when player stops moving
        transform.eulerAngles = Vector3.zero;
        isMovingFastEnough = false;
    }

    if (isMovingFastEnough)
    {
        float fovAngle = Mathf.Atan2(moveX, moveY) * Mathf.Rad2Deg;
        FOV.fovRotation = fovAngle;
    }
    FOV.DrawFieldofView();
}

    private void playStepSound(){
        if(!isStealth){
        int randomFootstep = UnityEngine.Random.Range(0,3);
        footsteps[randomFootstep].Play();
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
        punchCollider.enabled = true;

        movement.SetTrigger("Punch");
        Invoke("disablepunch",2.0f);


    }
    private void disablepunch()
    {
        punchCollider.enabled = false;
    }
    
     private void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Attacker"))
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(10);
        }
        else
        {
            Debug.LogWarning("Enemy component not found on Attacker, fool");
        }
    }
}


    private void ToggleStealth()
    {
        isStealth = !isStealth;
        movement.SetBool("isStealthy", isStealth);
    }
}
