using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour, IDamageable
{
    private PlayerControl _player;
    public float maxHealth;
    private float _currentHealth;
    private float _timer = 0f;
    public float CurrentHealth
    {
        get { return _currentHealth; }
    }
    private void Awake()
    {
        _player = transform.parent.GetComponent<PlayerControl>();
    }
    private void Start()
    {
        maxHealth = _player.stats.maxHealth;
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        if(_player.healthRegen > 0)
        {
            _timer += Time.deltaTime;
            if (_timer >= _player.timeToHeal)
            {
                Heal(_player.healthRegen);
                _timer = 0f;
            }
        }
    }
    public void AirDamageable()
    {
        throw new System.NotImplementedException();
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;

        _player.healPixel.Spawn(_player.transform.position + new Vector3(0f, 2f, 0f), amount);

        if (_currentHealth >= maxHealth)
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
