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
        EnemyBase.transform.LookAt(EnemyBase.Player.transform.position);
        Use();

    }

    protected override void AttackAction()
    {
        base.AttackAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!EnemyBase.attackInterrumpted)
        {
            other.GetComponent<IDamageable>().TakeDamage(_currentDamage);
        }
        else
        {
            EnemyBase.attackInterrumpted = false;
        }
    }
}
