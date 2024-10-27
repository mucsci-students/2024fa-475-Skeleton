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
    [Range(0,360)]
    public float fovRotation;
    public LayerMask targetMask; // The layer where the targets of the current FOV entity lie (player for guards, pickups for player)
    public LayerMask obstacleMask; // The layer containing obstacles we cannot see past (walls, etc.)

    
    public List<Transform> visibleTargets = new List<Transform>(); // A list of all positions of any currently visible targets

    // The following variables define how detailed the FOV cone is
    public float meshResolution; // Number of rays per degree
    public int edgeResolveIterations;
    public float edgeDistThreshold;

    public MeshFilter viewMeshFilter;
    private Mesh viewMesh;
    public bool targetAcquired = false;

        // Stores information about a single raycast
    public struct ViewCastInfo
    {
        public bool hit; // Did the ray hit anything?
        public Vector3 point; // Endpoint of the ray
        public float dist;  // Length of the ray
        public float angle; // Angle the ray was fired at

        // Constructor for the struct
        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle) {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;
        }
    }

    // Stores information about a single edge between two points in space
    public struct EdgeInfo 
    {
        public Vector3 pointA; 
        public Vector3 pointB;
        public EdgeInfo(Vector3 _pointA, Vector3 _pointB) {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    void Start(){
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    // Lateupdates are called ONLY after the playercontroller update has been called
    void LateUpdate(){
        DrawFieldofView();
    }

    IEnumerator FindTargetsWithDelay(float delay){
        while(true){
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets(){
        visibleTargets.Clear(); // Assert that we don't add any target twice
        targetAcquired = false; // And that we aren't stuck looking for something from a past life
        Collider2D[] targetsInCircle = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), viewRadius, targetMask);
        
        for (int i = 0; i < targetsInCircle.Length; i++) {
            Transform target = targetsInCircle[i].transform;
            
            Vector2 dirToTarget = (target.position - transform.position).normalized;
            
            // Check if target is within view angle
            if (Vector2.Angle(new Vector2(Mathf.Sin(fovRotation * Mathf.Deg2Rad), Mathf.Cos(fovRotation * Mathf.Deg2Rad)), dirToTarget) < viewAngle / 2) {
                float distToTarget = Vector3.Distance (transform.position, target.position);
            
                // Check if target is within line of sight (no obstacles blocking)
                if (!Physics2D.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask)) {
                    visibleTargets.Add(target);
                    targetAcquired = true;
                }
            } 
        }
    }

    public void DrawFieldofView(){
        int rayCount = Mathf.RoundToInt(viewAngle*meshResolution);
        float stepAngleSize = viewAngle/rayCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo ();
        int ray;
        for(ray = 0; ray<rayCount; ray++){
            float angle = fovRotation - viewAngle/2 + stepAngleSize * ray;
            ViewCastInfo newViewCast = ViewCast(angle);
            if(ray>0){
                bool edgeDistThresholdExceeded = Mathf.Abs(oldViewCast.dist - newViewCast.dist) > edgeDistThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistThresholdExceeded)) {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero) {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero) {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        // There are vertices for every viewpoint and the FOV entity itself
        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount-2)*3]; // The int array version of the mesh triangles where point coordinates are listed consecutively

        // Since our viewMesh is a character of the FOV entity object, all of the vertices need to be stored locally
        vertices[0] = Vector3.zero;
        int vertex;
        for (vertex = 0; vertex < vertexCount - 1; vertex++)
        {
            vertices[vertex + 1] = transform.InverseTransformPoint(viewPoints[vertex]);

            if (vertex < vertexCount - 2) {
                // Sets 3 variables in the int array per loop
                triangles[vertex * 3] = 0;
                triangles[vertex * 3 + 1] = vertex + 1;
                triangles[vertex * 3 + 2] = vertex + 2; 
            }
            
        }

        // Reset the viewMesh
        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();

    }

    // Binary search based algorithm to efficiently find obstacle cutoffs
    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) {
        // Initialize lower and upper bound for binary search
        float minAngle = minViewCast.angle; 
        float maxAngle = maxViewCast.angle; 
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast (angle);

            bool edgeDistThresholdExceeded = Mathf.Abs(minViewCast.dist - newViewCast.dist) > edgeDistThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDistThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);

    }

    ViewCastInfo ViewCast(float globalAngle) {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hit;

        hit = Physics2D.Raycast(transform.position,dir, viewRadius, obstacleMask);
        if (hit.collider!=null)
        {
            // If we hit an obstacle
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal){
        if(!angleIsGlobal){
            angleInDegrees += fovRotation;
        }
        return new Vector3(Mathf.Sin(angleInDegrees*Mathf.Deg2Rad), Mathf.Cos(angleInDegrees*Mathf.Deg2Rad),0);
    }
}
