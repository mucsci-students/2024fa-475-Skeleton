using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The generic state object that enemies will have for their various tasks
public abstract class EnemyState
{
    public abstract void EnterState(Enemy enemy);
    public abstract void Update(Enemy enemy);
    
}
