using UnityEngine;

public class AttackState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        enemy.attackHolder.UseAbility(0);
    }
    public override void UpdateState(Enemy enemy)
    {
        if(enemy.attackHolder.attacks[0].IsAtkAnimaitonOver() && enemy.movements.InRangeToChase() && enemy.movements.DistanceToPlayer() > 4)
        {
            enemy.events.Following();
        }
        else if(enemy.attackHolder.attacks[0].IsAtkAnimaitonOver() && !enemy.movements.InRangeToChase())
        {
            enemy.events.Idle();
        }
        if (enemy.attackHolder.attacks.Count > 0)
        {
            if (enemy.attackHolder.attacks[0].IsInRangeToAttack(_enemy.movements.DistanceToPlayer()) && enemy.attackHolder.attacks[0].IsReadToUse() && enemy.canAttack)
            {
                enemy.attackHolder.UseAbility(0);
            }
        }
    }

    public override void ExitState(Enemy enemy)
    {
        Debug.Log("Exit State Attack");

        base.ExitState(enemy);
    }
}
