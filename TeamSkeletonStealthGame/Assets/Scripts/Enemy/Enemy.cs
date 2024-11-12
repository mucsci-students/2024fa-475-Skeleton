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

    private Animator enemyMovement; //enemy animation controller
    float latestSpotTime = 0; // Keeps track of the last time this enemy saw the player
    public Material fovMaterial;
    public Material fovCombatMaterial;

    // Resetting our vaiables
    public Vector3 StartPos {get; private set;}
    public Quaternion StartRot {get; private set;}
    public Collider2D col;

    public FieldOfView FOV{get; private set;}
    public MeshRenderer RenderFOV{get;private set;}
    private SpriteRenderer spr; //used in takedamage to flash enemy sprite red when hit
    private Rigidbody2D rb;

    // Assign appropriate variables that will be uninitialized in editor
    protected virtual void Awake(){ //protected so only visible in this class, virtual so we can override it depending on enemy type if we decide to implement that
        FOV = GetComponent<FieldOfView>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
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
        //check if we detect the player and go to combative state if we do\
        if(FOV.visibleTargets.Count>0)// If a target (the player probably) is seen
        { //COMBAT MODE
            SetSpeed(0.001f);
            // Move from the enemy's current position to wherever the spotted target is
            Vector3 startPosition = transform.position;
            Vector3 endPosition = FOV.visibleTargets[0].transform.position;
            float pathLength = Vector2.Distance(startPosition, endPosition);
            float totalTimeForPath = pathLength / speed;
            float currentTimeOnPath = Time.time - latestSpotTime;
            transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

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

    public void TakeDamage(int damage)
    {
        
        this.hp -= damage;
        if (hp <= 0)
                Die();
        spr.color = Color.red;
        // Reset color after a short delay
        Invoke(nameof(ResetColor), 0.1f); // Adjust delay if needed
        float knockbackForce = 5f; // Adjust this value to control the intensity of the knockback
        rb.AddForce(new Vector2(2.0f,2.0f) * knockbackForce, ForceMode2D.Impulse);

        
    }

    private void ResetColor()
    {
        // Reset to original color
        spr.color = Color.white; // Assuming the original color is white
    }
    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("PlayerJab"))
        {
            
            TakeDamage(10);
        }
        if (col.gameObject.CompareTag("PlayerKick"))
        {
            
            TakeDamage(25);
        }
    }

    public void Die(){
        Invoke("SpawnEnemy", 1f);
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