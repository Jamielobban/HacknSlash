using UnityEngine;

public class ChaseState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);

    }
    public override void UpdateState(Enemy enemy)
    {
        enemy.movements.HandleFollow();
        if(!enemy.movements.InRangeToChase())
        {
            enemy.events.Idle();
        }
        //Check range to attack
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
