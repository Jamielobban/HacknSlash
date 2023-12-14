using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        enemy.controller.movements.EnableMovement();
    }
    public override void UpdateState(Enemy enemy)
    {
        enemy.controller.movements.HandlePatrollBetweenPoints();
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
