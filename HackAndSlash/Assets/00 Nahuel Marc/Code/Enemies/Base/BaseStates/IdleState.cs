using UnityEngine;

public class IdleState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
    }
    public override void UpdateState(Enemy enemy)
    {
        if(enemy.movements.InRangeToChase())
        {
            enemy.events.Following();
        }
        //else if(enemy.movements.)
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
