using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageable
{
    protected EnemyController _enemy;
    protected float _currentHealth;
    public CharacterStat maxHealth = new CharacterStat();

    protected virtual void Awake()
    {
        _enemy = transform.parent.GetComponent<EnemyController>();
        _currentHealth = maxHealth.Value;
    }

    public virtual void Die()
    {
        //StartRespawnHandler
        //SetVFX
        //Sound? etc.
        throw new System.NotImplementedException();
    }

    public virtual void Heal(float amount)
    {
        _currentHealth += amount;
        if(_currentHealth > maxHealth.Value)
        {
            _enemy.hud.UpdateHealthBar(_currentHealth, maxHealth.Value);
            _currentHealth = maxHealth.Value;
        }
        _enemy.hud.UpdateHealthBar(_currentHealth, maxHealth.Value);
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if(_currentHealth <= 0)
        {
            _enemy.hud.UpdateHealthBar(_currentHealth, maxHealth.Value);
            Die();
        }
        _enemy.hud.UpdateHealthBar(_currentHealth, maxHealth.Value);

    }

}
