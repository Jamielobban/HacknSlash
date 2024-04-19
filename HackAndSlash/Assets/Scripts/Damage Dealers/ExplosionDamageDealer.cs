public class ExplosionDamageDealer : DamageDealer
{
    public float timeToDestroyDamageable;
    protected override void Awake() => Invoke(nameof(DestroyParticle), timeToDestroyDamageable);
    private void DestroyParticle() => Destroy(gameObject);
}
