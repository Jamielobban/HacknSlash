using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttackState : AttackState
{
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        _enemy.events.OnHit += () => { _enemy.attackInterrupted = true; };
    }
    public override void UpdateState(Enemy enemy)
    {
        if (enemy.attackHolder.attacks[0].IsAtkAnimaitonOver() && enemy.movements.InRangeToChase() && enemy.movements.DistanceToPlayer() > 2.5f)
        {
            enemy.events.Following();
        }
        else if (enemy.attackHolder.attacks[0].IsAtkAnimaitonOver() && !enemy.movements.InRangeToChase())
        {
            enemy.events.Idle();
        }

        if (enemy.attackHolder.attacks.Count > 0)
        {
            if (enemy.attackHolder.attacks[0].IsInRangeToAttack(_enemy.movements.DistanceToPlayer()) && enemy.attackHolder.attacks[0].IsReadToUse() && enemy.canAttack)
            {
                HandleRotationFacePlayer();

                enemy.attackHolder.UseAbility(0);
            }
        }
    }
}
