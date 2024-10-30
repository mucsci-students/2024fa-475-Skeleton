using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyState : EnemyState
{

    float stopDuration;
    
    public override void EnterState(Enemy enemy)
    {
        enemy.Agent.speed = enemy.speed;
        enemy.RenderFOV.material = enemy.fovMaterial;

        PatrollingEnemy patrollingEnemy = (PatrollingEnemy) enemy;
        stopDuration = patrollingEnemy.waitDuration;
        enemy.Agent.destination = patrollingEnemy.GetStopPosition().position;


    }

    // Update is called once per frame
    public override void Update(Enemy enemy)
    {   
        // Wait it out until we've spent all our time at this post, then reset the timer and move on
        if(stopDuration<0){ 
            PatrollingEnemy patrollingEnemy = (PatrollingEnemy) enemy;
            stopDuration = patrollingEnemy.waitDuration; //reset timer
            patrollingEnemy.NextStop(); //move to next stop
            enemy.Agent.destination = patrollingEnemy.GetStopPosition().position; //update new destination
        }
        else stopDuration -= Time.deltaTime;
    }
}
