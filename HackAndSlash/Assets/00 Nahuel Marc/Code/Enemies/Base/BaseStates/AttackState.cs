using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
    }
    public override void UpdateState(Enemy enemy)
    {
    }

    public override void ExitState(Enemy enemy)
    {
        Debug.Log("Exit State Attack");

        base.ExitState(enemy);
    }
}
