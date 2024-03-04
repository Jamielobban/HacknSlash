using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttackState : AttackState
{
    public override void UpdateState(Enemy enemy)
    {
        HandleRotationFacePlayer();
        if (enemy.attackHolder.attacks[0].IsAtkAnimaitonOver() && enemy.movements.InRangeToChase() && enemy.movements.DistanceToPlayer() > 3)
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
                enemy.attackHolder.UseAbility(0);
            }
        }
    }
}
