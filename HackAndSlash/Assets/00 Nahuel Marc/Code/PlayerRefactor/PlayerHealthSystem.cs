using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour, IDamageable
{
    public float maxHealth;
    private float _currentHealth;
    public float CurrentHealth
    {
        get { return _currentHealth; }
    }
    private void Awake()
    {
        _currentHealth = maxHealth;
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
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    public void Die()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
