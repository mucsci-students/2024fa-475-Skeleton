using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingEnemy : Enemy
{
    
    public float waitDuration = 2f;

    [SerializeField] protected Transform[] patrolStops;
    int currentStop;


    // Start is called before the first frame update
    protected override void Start()
    {
        currentStop = 0;
        base.Start();
        startState = patrolState;
        TransitionToState(startState);
    }

    public void NextStop(){
        if(currentStop == patrolStops.Length-1)
            currentStop = 0;
        else currentStop ++;

    }

    public Transform GetStopPosition(){
        return patrolStops[currentStop];
    }
}
