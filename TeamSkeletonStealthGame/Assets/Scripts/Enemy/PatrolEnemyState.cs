using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyState : EnemyState
{

    float stopDuration;

    // Start is called before the first frame update
    public override void EnterState(Enemy enemy)
    {
        enemy.agent.speed = enemy.speed;
        enemy.renderFOV.material = enemy.fovMaterial;

        PatrollingEnemy patrollingEnemy = (PatrollingEnemy) enemy;
        stopDuration = patrollingEnemy.waitDuration;
        enemy.agent.destination = patrollingEnemy.GetStopPosition().position;


    }

    // Update is called once per frame
    public override void Update(Enemy enemy)
    {   
        // Wait it out until we've spent all our time at this post, then reset the timer and move on
        if(stopDuration<0){ 
            PatrollingEnemy patrollingEnemy = (PatrollingEnemy) enemy;
            stopDuration = patrollingEnemy.waitDuration; //reset timer
            patrollingEnemy.NextStop(); //move to next stop
            enemy.agent.destination = patrollingEnemy.GetStopPosition().position; //update new destination
        }
        else stopDuration -= Time.deltaTime;
    }
}
