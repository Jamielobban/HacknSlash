using DamageNumbersPro;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    public float currentMaxHealth { get; set; }
    private float _currentHealth;
    private Enemy _enemy;
    public EnemyHUD hud;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    
    private void Awake()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
        currentMaxHealth = _maxHealth;
        _currentHealth = currentMaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_enemy.IsDead)
        {
            return;
        }
        _currentHealth -= damage;
        
        _enemy.EnemyFSM.Trigger(Enums.StateEvent.HitEnemy);
        hud.UpdateHealthBar(_currentHealth, currentMaxHealth);
        
        if (_currentHealth <= 0)
        {
            _enemy.IsDead = true;
            _enemy.OnDespawnEnemy();
        }
    }

    public void Heal(float amount)
    {
        if (_enemy.IsDead)
        {
            return;
        }
        
        _currentHealth += amount;
        
        if(_currentHealth > currentMaxHealth)
        {
            _currentHealth = currentMaxHealth;
        }
        hud.UpdateHealthBar(_currentHealth, currentMaxHealth);
    }

    public void ResetEnemyHealth()
    {
        _currentHealth = currentMaxHealth;
        hud.UpdateHealthBar(_currentHealth, currentMaxHealth);
    }

}
