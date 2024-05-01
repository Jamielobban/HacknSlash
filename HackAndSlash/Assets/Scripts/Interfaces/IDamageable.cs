using DamageNumbersPro;

public interface IDamageable
{
    bool IsPlayer();
    void TakeDamage(float damage, DamageNumber visualEffect);
    void ApplyBurn();
    void ApplyStun(float time);
    void Heal(float amount);
}

