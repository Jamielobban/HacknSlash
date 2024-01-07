using UnityEngine;

public class AttackState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        Debug.Log("Enter Attack");
        enemy.attackHolder.UseAbility(0);
    }
    public override void UpdateState(Enemy enemy)
    {
        if(enemy.attackHolder.attacks[0].IsInCd() && enemy.movements.InRangeToChase())
        {
            enemy.events.Following();
        }
        else if(enemy.attackHolder.attacks[0].IsInCd() && !enemy.movements.InRangeToChase())
        {
            enemy.events.Idle();
        }
    }

    public override void ExitState(Enemy enemy)
    {
        Debug.Log("Exit State Attack");

        base.ExitState(enemy);
    }
}
