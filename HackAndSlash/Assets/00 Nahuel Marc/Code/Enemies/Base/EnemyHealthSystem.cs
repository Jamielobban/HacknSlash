using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageable
{
    protected Enemy _enemy;
    protected float _currentHealth;
    public CharacterStat maxHealth = new CharacterStat();

    protected virtual void Awake()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
        _currentHealth = maxHealth.Value;

    }

    protected virtual void Start()
    {
        _enemy.events.OnHeal += ChangeLife;
        _enemy.events.OnHit += ChangeLife;
    }

    public virtual void Stun(float stunTime)
    {
        _enemy.events.OnStun += () => _enemy.SetState(new StunState(stunTime));
        _enemy.events.Stun();
    }

    public virtual void Heal(float amount)
    {
        _currentHealth += amount;
        if(_currentHealth > maxHealth.Value)
        {
            _currentHealth = maxHealth.Value;
        }
        _enemy.events.Heal();
    }

    public virtual void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _enemy.events.Hit();
        if(_currentHealth <= 0)
        {
            _enemy.events.Die();
        }
    }

    public virtual void Impact()
    {
        //Si throw air true -> set state air
        _enemy.SetState(new AirState());
    }

    private void ChangeLife()
    {
        _enemy.hud.UpdateHealthBar(_currentHealth, maxHealth.Value);
    }
}
