using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemyState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.agent.speed = 0;
        enemy.renderFOV.material = enemy.fovMaterial; //get rid of the damn pink lol
    }

    public override void Update(Enemy enemy)
    {
        //dostuff, since this is idle script i dont think it needs anything
    }
}
