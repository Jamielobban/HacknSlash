using DamageNumbersPro;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    public float currentMaxHealth { get; set; }
    private float _currentHealth;
    private EnemyBase _enemyBase;
    public EnemyHUD hud;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;
    
    private void Awake()
    {
        _enemyBase = transform.parent.GetComponent<EnemyBase>();
        currentMaxHealth = _maxHealth;
        _currentHealth = currentMaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_enemyBase.IsDead)
        {
            return;
        }
        _currentHealth -= damage;
        
        _enemyBase.EnemyFSM.Trigger(Enums.StateEvent.HitEnemy);
        hud.UpdateHealthBar(_currentHealth, currentMaxHealth);
        
        if (_currentHealth <= 0)
        {
            _enemyBase.IsDead = true;
            _enemyBase.OnDespawnEnemy();
        }
    }

    public void Heal(float amount)
    {
        if (_enemyBase.IsDead)
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
