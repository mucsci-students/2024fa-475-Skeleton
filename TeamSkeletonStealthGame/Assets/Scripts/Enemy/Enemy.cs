using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// General behavior of an enemy: see readme for FSM breakdown
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))] //these statements actually add the component in the editor for you if not there already
public class Enemy : MonoBehaviour
{
    
    public float speed = 2f;
    public int hp = 50;

    protected Animator emv; //enemy animator
    float latestSpotTime = 0; // Keeps track of the last time this enemy saw the player
    public Material fovMaterial;
    public Material fovCombatMaterial;

    // Resetting our vaiables
    public Vector3 StartPos {get; private set;}
    public Quaternion StartRot {get; private set;}
    public Collider2D col;
    public Collider2D attack;

    public FieldOfView FOV{get; private set;}
    public MeshRenderer RenderFOV{get;private set;}
    private SpriteRenderer spr; //used in takedamage to flash enemy sprite red when hit

    //declaring before to avoid reallocating memory every update call
    private Rigidbody2D rb; Vector3 startPosition; Vector3 endPosition;
    float pathLength;float totalTimeForPath;float currentTimeOnPath;



    // Assign appropriate variables that will be uninitialized in editor
    protected virtual void Awake(){ //protected so only visible in this class, virtual so we can override it depending on enemy type if we decide to implement that
        FOV = GetComponent<FieldOfView>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        emv = GetComponent<Animator>();
        attack = transform.Find("MeleeCollider").GetComponent<Collider2D>();
        attack.enabled = false; 

        rb.gravityScale = 0;
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        RenderFOV = FOV.viewMeshFilter.GetComponent<MeshRenderer>();
    }

    protected virtual void Start()
    {
        StartPos = transform.position;
        StartRot = transform.rotation;
    }

    protected virtual void Update(){
        bool isMoving = rb.velocity.magnitude > 0.01f;
        emv.SetBool("isMoving", isMoving); 

        //check if we detect the player and go to combative state if we do\
        if(FOV.visibleTargets.Count>0)// If a target (the player probably) is seen
        { //COMBAT MODE
            SetSpeed(0.001f);
            // Move from the enemy's current position to wherever the spotted target is
            startPosition = transform.position;
            endPosition = FOV.visibleTargets[0].transform.position;
            pathLength = Vector2.Distance(startPosition, endPosition);
            totalTimeForPath = pathLength / speed;
            currentTimeOnPath = Time.time - latestSpotTime;
            transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

            if (Vector2.Distance(transform.position, endPosition) <= 1.0f)
                Attack();
        }
    }


    public GameObject alertPrefab; // Reference to the exclamation point prefab (set in Inspector)
    private GameObject alertInstance; // Priv reference to the instance of the alert
    public float alertDuration = 2f; // Keep alert symbol up for 2 seconds
    private bool soundPlayed = true;
    protected virtual void LateUpdate()
    {
        if (FOV.visibleTargets.Count > 0 && !soundPlayed) // Check if there are visible targets and sound hasn't been played
        {
            GenerateAlert();
        }
        else if (FOV.visibleTargets.Count == 0 && soundPlayed) // Reset the flag when no targets are visible
        {
            soundPlayed = false;
            if(alertInstance!=null){
                Destroy(alertInstance);
                alertInstance = null;
            }
        }
    }

    public void GenerateAlert()
    {
        //Invoke("SpawnEnemy", 2f);
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
            soundPlayed = true;  // Set flag to prevent re-triggering
        }

        // Show the exclamation point above the enemy's head
        // Spawn the alert image a little above the enemy's position
        Vector3 alertPosition = transform.position + new Vector3(0, 1.23f, 0); // Adjust the offset as needed
        GameObject alertInstance = Instantiate(alertPrefab, alertPosition, Quaternion.identity);

        // Add the helper follow script to keep the alert above the enemy
        alertInstance.AddComponent<FollowEnemy>().target = this.transform;

        // Destroy the alert image after the specified duration
        Destroy(alertInstance, alertDuration);
    }

    public void SetSpeed(float newSpeed){
        speed = newSpeed;
    }

    public void Attack()
    {
        emv.SetTrigger("Shoot");
    }


    
    public void TakeDamage(int damage)
    {
        hp -= damage;
        emv.SetTrigger("Hit");

        if (hp <= 0)
        {
            Die();
            return; 
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector2 playerPos = player.transform.position;
            float force = 5f;
            Vector2 direction = ((Vector2)transform.position - playerPos).normalized;
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }

    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("PlayerJab"))
            TakeDamage(15);
        else if (col.gameObject.CompareTag("PlayerKick"))
            TakeDamage(30);
        
    }

    public void Die()
    {
        
        emv.SetTrigger("Die");
        Invoke("SpawnEnemy", 1f);

        Destroy(gameObject, 2.5f);

    }

}
public class FollowEnemy : MonoBehaviour
{
    public Transform target; // The enemy this alert should follow
    public Vector3 offset = new Vector3(0, 1.23f, 0); // Adjust as needed

    void Update()
    {
        if (target != null)
        {
            // Set the position to follow the target with an offset
            transform.position = target.position + offset;
        }
    }
}