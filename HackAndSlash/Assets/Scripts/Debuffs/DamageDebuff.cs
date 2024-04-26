using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDebuff : Debuff
{
    protected float _timeToDamage;
    protected float _damage;

    protected float _currentTickTime = 0f;

    void Start()
    {
        
    }
    protected override void IsActiveUpdate()
    {
        _currentTickTime += Time.deltaTime;

        base.IsActiveUpdate();

        if(_currentTickTime >= _timeToDamage)
        {
          //  enemy.HealthSystem.TakeDamage(_damage);
        }
    }

    public void SetDamage(float value) => _damage = value;
}
