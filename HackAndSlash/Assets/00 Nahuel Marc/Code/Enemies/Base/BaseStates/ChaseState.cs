using UnityEngine;

public class ChaseState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        enemy.controller.movements.EnableMovement();
    }
    public override void UpdateState(Enemy enemy)
    {
        enemy.controller.movements.HandleFollow();
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
