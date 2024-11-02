using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Enemy
{
    
    public float waitDuration = 2f;

    public GameObject[] patrolStops;
    int currentStop;
    int nextStop; 
    float latestCheckInTime; // Identifies the last time this enemy reached its stop


    // Start is called before the first frame update
    protected override void Start()
    {
        currentStop = 0;
        nextStop = 1;
        latestCheckInTime = Time.time; // Initialize this enemie's (zero'th) stop check in
        base.Start();
    }

    
    // Update is called once per frame, very resource intensive
    protected override void Update()
    {
        
        // Wait it out until we've spent all our time at this post, then reset the timer and move on
        if(!FOV.targetAcquired){
    if(waitDuration<=0){ 
        Vector3 startPosition = patrolStops[currentStop].transform.position;
        Vector3 endPosition = (currentStop < patrolStops.Length - 1)?patrolStops[nextStop].transform.position:patrolStops[0].transform.position;
        
        
        // Calculating next stop/route to stop
        float pathLength = Vector2.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / speed;
        float currentTimeOnPath = Time.time - latestCheckInTime;

        
        // Moving to next stop        
        gameObject.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
        Vector2 direction = (gameObject.transform.position - startPosition).normalized;

        float fovangle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg; //trig go brr
        
        FOV.fovRotation = fovangle;
        
        
        if (Vector2.Distance(gameObject.transform.position, endPosition) < 0.1f)
        {
            currentStop++;

            if (currentStop >= patrolStops.Length)
            {
                currentStop = 0; // Go back to start
                latestCheckInTime = Time.time;
            }
            nextStop = (currentStop+1)%patrolStops.Length; //modulo saves out of bounds errors
            latestCheckInTime = Time.time;
        }
        // ADD COMBAT LOGIC HERE I THINK (basically just set destination to player regardless of waypoints)

    }
    else waitDuration -= Time.deltaTime; // Otherwise keep waiting
    }
    }
}