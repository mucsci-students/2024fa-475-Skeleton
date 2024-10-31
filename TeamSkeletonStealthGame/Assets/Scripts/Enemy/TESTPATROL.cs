using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTPATROL : MonoBehaviour
{
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;

    // Use this for initialization
    void Start()
    {
        lastWaypointSwitchTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;
        
        float pathLength = Vector2.Distance(startPosition, endPosition);
        float totalTimeForPath = pathLength / speed;
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        gameObject.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
         
        if (gameObject.transform.position.Equals(endPosition))
        {
            if (currentWaypoint < waypoints.Length - 2)
            {
                // Switch to next waypoint
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
                // TODO: Rotate into correct move direction
            }
            else
            {
                currentWaypoint = 0;
            }
        }
        // ADD COMBAT LOGIC HERE I THINK (basically just set destination to player regardless of waypoints)

    }

}
