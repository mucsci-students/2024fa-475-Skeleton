using System;
using System.Collections;
using UnityEngine;


// Script to define field of view behavior
// Can be placed on player or enemy prefabs

// Logic Credit: Sebastian Lague on YouTube
public class FieldOfView : MonoBehaviour
{

    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal){
        if(!angleIsGlobal){
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Cos(angleInDegrees*Mathf.Deg2Rad), Mathf.Sin(angleInDegrees*Mathf.Deg2Rad),0);
    }
}
