using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombativeEnemy : Enemy
{
    
    protected override void Start()
    {
        base.Start();
        StartState = CombatState;
        TransitionToState(StartState);
    }

    
}
