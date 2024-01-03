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
    }

    public override void ExitState(Enemy enemy)
    {
        Debug.Log("Exit State Chase");

        base.ExitState(enemy);
    }
}
