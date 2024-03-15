using UnityEngine;

public class ChaseState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        _enemy.sound.monsterFootStep.volume = 1f;
        _enemy.sound.monsterFootStep.Play();
    }
    public override void UpdateState(Enemy enemy)
    {
        enemy.movements.HandleFollow();
        if (!enemy.movements.InRangeToChase())
        {
            enemy.events.Idle();
        }
        if (enemy.attackHolder.attacks.Count > 0)
        {
            if (enemy.attackHolder.attacks[0].IsInRangeToAttack(_enemy.movements.DistanceToPlayer()) && enemy.attackHolder.attacks[0].IsReadToUse() && enemy.canAttack)
            {
                enemy.events.Attacking();
            }
        }
    }
    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
        _enemy.sound.FadeFootSteps(.25f);
    }
}
