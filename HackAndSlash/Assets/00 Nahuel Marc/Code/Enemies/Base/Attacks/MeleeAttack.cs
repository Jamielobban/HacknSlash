using UnityEngine;

public class MeleeAttack : EnemyBaseAttack
{
    public Collider damageCollider;
    protected override void SetVisualEffects()
    {
        base.SetVisualEffects();
        damageCollider.enabled = true;
        Invoke("DisableCollider", 0.2f);
    }

    private void DisableCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerControl>()?.GetDamage(data.damage.Value);
    }
}
