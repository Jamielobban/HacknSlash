using DamageNumbersPro;
using UnityEngine;

public class DamageEnemiesDebuff : Debuff
{
    public EnemyBase enemy;

    public float _timeToDamage;
    protected float _damage;

    protected float currentTickTime = 0f;
    public DamageNumber damageNumber;

    void Start()
    {
        
    }
    protected override void IsActiveUpdate()
    {
        currentTickTime += Time.deltaTime;

        base.IsActiveUpdate();

        if(currentTickTime >= _timeToDamage)
        {
            if(_damage <= 1)
            {
                _damage = 1;
            }
            enemy.HealthSystem.TakeDamage(_damage, damageNumber);
            currentTickTime = 0f;
        }
    }
    public override void ApplyDebuff(float damage)
    {
        _damage = damage;
        base.ApplyDebuff(damage);
    }
    public void SetDamage(float value) => _damage = value;
}
