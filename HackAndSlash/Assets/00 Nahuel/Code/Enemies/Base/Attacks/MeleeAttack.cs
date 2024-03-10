using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class MeleeAttack : EnemyBaseAttack
{
    public Collider damageCollider;
    public List<AudioClip> fxAudios = new List<AudioClip>();     
    protected override void AttackAction()
    {
        base.AttackAction();
        AudioManager.Instance.PlayRandomFx(fxAudios);
        if (!enemy.attackInterrupted)
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
