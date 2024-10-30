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
    public EnemyState CurrentState {get; protected set;}
    public EnemyState StartState {get; protected set;}
    public IdleEnemyState IdleState = new IdleEnemyState();
    public PatrolEnemyState PatrolState = new PatrolEnemyState();
    public CombatEnemyState CombatState = new CombatEnemyState();


    // Resetting our vaiables
    public Vector3 StartPos {get; private set;}
    public Quaternion StartRot {get; private set;}
    #endregion


    public NavMeshAgent Agent{get; private set;}
    public FieldOfView FOV{get; private set;}
    public MeshRenderer RenderFOV{get;private set;}

    // Assign appropriate variables that will be uninitialized in editor
    protected virtual void Awake(){ //protected so only visible in this class, virtual so we can override it depending on enemy type if we decide to implement that
        Agent = GetComponent<NavMeshAgent>();
        FOV = GetComponent<FieldOfView>();
        RenderFOV = FOV.viewMeshFilter.GetComponent<MeshRenderer>();
    }



    protected virtual void Start()
    {
        StartPos = transform.position;
        StartRot = transform.rotation;
    }

    // Allows us to easily switch from one state to another
    public void TransitionToState(EnemyState newState){
        CurrentState = newState;
        CurrentState.EnterState(this);
    }

    protected virtual void Update(){
        //check if we detect the player and go to combative state if we do\
        if(this.FOV.targetAcquired){
            TransitionToState(CombatState);
        }
        CurrentState.Update(this);
    }


    public void SetSpeed(float newSpeed){
        Agent.speed = newSpeed;
    }

    public void Die(){
        Destroy(gameObject);
    }

}
