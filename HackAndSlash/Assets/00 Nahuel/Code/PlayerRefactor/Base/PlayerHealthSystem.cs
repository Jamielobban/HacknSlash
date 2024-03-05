using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour, IDamageable
{
    private PlayerControl _player;
    float maxHealth;
    private float _currentHealth;
    public float CurrentHealth
    {
        get { return _currentHealth; }
    }
    private void Awake()
    {
        maxHealth = _player.stats.maxHealth;
        _currentHealth = maxHealth;
        _player = transform.parent.GetComponent<PlayerControl>();
    }

    public void AirDamageable()
    {
        throw new System.NotImplementedException();
    }

    public void Heal(float amount)
    {
        _currentHealth -= amount;

        if(_currentHealth >= maxHealth)
        {
            _currentHealth = maxHealth;
        }
        _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if(_player.states == PlayerControl.States.HIT || _player.states == PlayerControl.States.DASH || _player.states == PlayerControl.States.DEATH)
        {
            return;
        }

        _player.states = PlayerControl.States.HIT;

        _player.HitEffect();

        _currentHealth -= damage;

        if(_currentHealth <= 0)
        {
            _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
            _currentHealth = 0;
            Die();
        }
        _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
    }

    public void Die()
    {
        _player.DeadEffect();
    }
}
