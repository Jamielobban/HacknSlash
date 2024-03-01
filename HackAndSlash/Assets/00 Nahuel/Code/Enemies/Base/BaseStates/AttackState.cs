using UnityEngine;

public class AttackState : EnemyState
{
    private GameObject _player;
    public override void EnterState(Enemy enemy)
    {
        base.EnterState(enemy);
        enemy.attackHolder.UseAbility(0);
        _player = FindObjectOfType<PlayerManager>().gameObject;
    }
    public override void UpdateState(Enemy enemy)
    {
        HandleRotationFacePlayer();
        if (enemy.attackHolder.attacks[0].IsAtkAnimaitonOver() && enemy.movements.InRangeToChase() && enemy.movements.DistanceToPlayer() > 3)
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

    private void HandleRotationFacePlayer()
    {
        Quaternion rotation = Quaternion.LookRotation((_enemy.movements.target.transform.position - _enemy.transform.position));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
    }

    public override void ExitState(Enemy enemy)
    {
        base.ExitState(enemy);
    }
}
