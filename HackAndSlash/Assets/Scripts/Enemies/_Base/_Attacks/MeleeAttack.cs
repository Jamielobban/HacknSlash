using System;
using UnityEngine;
using UnityHFSM;

[RequireComponent(typeof(Collider))]
public class MeleeAttack : BaseEnemyAttack
{
    private Collider _colliderDamage;

    protected override void Awake()
    {
        base.Awake();
        _colliderDamage = GetComponent<Collider>();
        _colliderDamage.enabled = false;
    }

    public void OnAttack(State<Enums.EnemyStates, Enums.StateEvent> state)
    {
        _enemy.transform.LookAt(_enemy.Player.transform.position);
        Use();

    }

    protected override void AttackAction()
    {
        base.AttackAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_enemy.attackInterrumpted)
        {
            other.GetComponent<IDamageable>().TakeDamage(_currentDamage);
        }
        else
        {
            _enemy.attackInterrumpted = false;
        }
    }
}
