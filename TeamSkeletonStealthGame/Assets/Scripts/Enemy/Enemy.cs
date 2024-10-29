using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


// General behavior of an enemy: see readme for FSM breakdown
public class Enemy : MonoBehaviour
{
    
    public float speed = 2f;
    public Material fovMaterial;

    #region State Machine
    public EnemyState currentState {get; protected set;}
    public EnemyState startState {get; protected set;}
    public IdleEnemyState idleState = new IdleEnemyState();
    public PatrolEnemyState patrolState = new PatrolEnemyState();

    // Resetting our vaiables
    public Vector3 startPos {get; private set;}
    public Quaternion startRot {get; private set;}
    #endregion


    public NavMeshAgent agent{get; private set;}
    public FieldOfView fov{get; private set;}
    public MeshRenderer renderFOV{get;private set;}

    // Assign appropriate variables that will be uninitialized in editor
    protected virtual void Awake(){ //protected so only visible in this class, virtual so we can override it depending on enemy type if we decide to implement that
        agent = GetComponent<NavMeshAgent>();
        fov = GetComponent<FieldOfView>();
        renderFOV = fov.viewMeshFilter.GetComponent<MeshRenderer>();
    }



    protected virtual void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    // Allows us to easily switch from one state to another
    public void TransitionToState(EnemyState newState){
        currentState = newState;
        currentState.EnterState(this);
    }

    protected virtual void Update(){
        //check if we detect the player and go to combative state if we do
        //currentState.Update(this);
    }


    public void SetSpeed(float newSpeed){
        agent.speed = newSpeed;
    }

    public void Die(){
        Destroy(gameObject);
    }

}
