using UnityEngine;

public class ChaseState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        Debug.Log("Enter Chase");

    }
    public override void UpdateState(Enemy enemy)
    {
        enemy.movements.HandleFollow();
        if(!enemy.movements.InRangeToChase())
        {
            enemy.events.Idle();
        }
        if(InRangeToAttack(0) && enemy.attackHolder.attacks[0].IsReadToUse() && enemy.canAttack)
        {
            enemy.events.Attacking();
        }
    }
    private bool InRangeToAttack(int v) => _enemy.movements.DistanceToPlayer() <= _enemy.attackHolder.attacks[v].data.range.Value;
    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
