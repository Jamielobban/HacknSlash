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
        _enemyBase.transform.LookAt(_enemyBase.target.transform.position);
        Use();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (!_enemyBase.attackInterrumpted)
        {
            if (_enemyBase.target == _enemyBase.Player.transform && damageable.IsPlayer())
            {
                damageable.TakeDamage(_currentDamage);
            }
            else if (_enemyBase.target != _enemyBase.Player.transform && !damageable.IsPlayer())
            {
                damageable.TakeDamage(_currentDamage);
            }
        }
        else
        {
            _enemyBase.attackInterrumpted = false;
        }
    }
}
