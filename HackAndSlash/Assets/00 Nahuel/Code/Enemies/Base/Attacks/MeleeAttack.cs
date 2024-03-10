using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : EnemyBaseAttack
{
    public Collider damageCollider;


    protected override void AttackAction()
    {
        base.AttackAction();
        if(!enemy.attackInterrupted)
        {
            damageCollider.enabled = true;
            Invoke("DisableCollider", 0.2f);
        }
    }

    private void DisableCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>()?.TakeDamage(data.damage.Value);
    }
}
