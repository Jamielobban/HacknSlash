using DamageNumbersPro;

public interface IDamageable
{
    bool IsPlayer();
    void TakeDamage(float damage, DamageNumber visualEffect);
    void ApplyPoison();
    void ApplyBleed();
    void ApplyBurn();
    void Heal(float amount);
}

