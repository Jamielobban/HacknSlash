public interface IDamageable
{
    bool IsPlayer();
    void TakeDamage(float damage);
    void Heal(float amount);
}

