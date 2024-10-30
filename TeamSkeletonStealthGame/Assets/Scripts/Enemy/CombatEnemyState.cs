using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatEnemyState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.SetSpeed(10f); // fast boi
        enemy.RenderFOV.material = enemy.fovMaterial; //could update with new (maybe red?) material here
    }

    // Update is called once per frame
    public override void Update(Enemy enemy)
    {   
        if(enemy.FOV.visibleTargets.Count>0){ // If a target (the player probably) is seen
            enemy.Agent.destination = enemy.FOV.visibleTargets[0].position; // Run after the first visible target in the list
        }
        else{
            enemy.TransitionToState(enemy.StartState); // Otherwise go back to whatever you were doing
        }
        
    }
}
