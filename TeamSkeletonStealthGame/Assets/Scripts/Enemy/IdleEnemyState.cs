using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEnemyState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        //enemy.Agent.speed = 0;
        enemy.RenderFOV.material = enemy.fovMaterial; //get rid of the damn pink lol
    }

    public override void Update(Enemy enemy)
    {
        
    }
}
