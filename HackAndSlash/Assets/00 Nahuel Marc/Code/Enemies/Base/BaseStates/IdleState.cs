using UnityEngine;

public class IdleState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        enemy.controller.movements.DisableMovement();
    }
    public override void UpdateState(Enemy enemy)
    {
        throw new System.NotImplementedException();
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
