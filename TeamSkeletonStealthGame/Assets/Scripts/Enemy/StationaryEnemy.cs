using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents an enemy who is not currently moving
public class StationaryEnemy : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        startState = idleState;
        TransitionToState(startState);
    }
}
