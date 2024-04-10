using System;
using UnityEngine;
using UnityHFSM;

[RequireComponent(typeof(Collider))]
public class MeleeAttack : BaseEnemyAttack
{
    private Collider _colliderDamage;
    public float timeToDeactiveCollider = 0.1f;

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
        _colliderDamage.enabled = true;
        Invoke(nameof(DeactiveCollider), timeToDeactiveCollider);
    }

    private void DeactiveCollider()
    {
        _colliderDamage.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>().TakeDamage(_currentDamage);
    }
}
