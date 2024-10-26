using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Script to define field of view behavior
// Can be placed on player or enemy prefabs

// Logic Credit: Sebastian Lague on YouTube
public class FieldOfView : MonoBehaviour
{

    public float viewRadius; // The radius of the view circle
    [Range(0,360)]
    public float viewAngle; // The FOV cone arc angle from "left eye" to "right eye"

    public LayerMask targetMask; // The layer where the targets of the current FOV entity lie (player for guards, pickups for player)
    public LayerMask obstacleMask; // The layer containing obstacles we cannot see past (walls, etc.)

    
    public List<Transform> visibleTargets = new List<Transform>(); // A list of all positions of any currently visible targets

    void Start(){
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    IEnumerator FindTargetsWithDelay(float delay){
        while(true){
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

void FindVisibleTargets(){
    visibleTargets.Clear(); // Assert that we don't add any target twice
    Collider2D[] targetsInCircle = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
    
    foreach (Collider2D targetCollider in targetsInCircle) {
        Transform target = targetCollider.transform;
        
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        
        // Check if target is within view angle
        if (Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2) { // Ensure correct direction vector here
            float distToTarget = Vector3.Distance(transform.position, target.position);
            
            // Check if target is within line of sight (no obstacles blocking)
            if (!Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) {
                visibleTargets.Add(target);
                
            }
        } 
    }
}

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal){
        if(!angleIsGlobal){
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees*Mathf.Deg2Rad), Mathf.Sin(angleInDegrees*Mathf.Deg2Rad),0);
    }
}
