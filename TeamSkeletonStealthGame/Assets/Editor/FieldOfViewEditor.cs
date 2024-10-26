using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (FieldOfView))]
public class EditorScript : Editor // Make sure it actually references the editor
{
    void OnSceneGUI(){
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.right, 360, fov.viewRadius);
    }
}
