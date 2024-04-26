using DamageNumbersPro;

public interface IDamageable
{
    bool IsPlayer();
    void TakeDamage(float damage, DamageNumber visualEffect);
    void Heal(float amount);
}

