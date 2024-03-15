
public interface IDamageableEnemy
{
    void TakeDamage(PlayerControl.HitState state, float damage);

    void AirDamageable();

    void Heal(float amount);

}
