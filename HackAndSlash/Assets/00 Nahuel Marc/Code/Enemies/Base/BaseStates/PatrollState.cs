using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);

    }
    public override void UpdateState(Enemy enemy)
    {
        enemy.movements.HandlePatrollInArea();
    }

    public override void ExitState(Enemy enemy)
    {
        Debug.Log("Exit State Patroll");

        base.ExitState(enemy);
    }
}
