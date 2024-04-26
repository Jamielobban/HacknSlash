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

    private Collider _getDamageCollider;
    public Collider GetDamageCollider => _getDamageCollider;
    private bool _isPlayer = false;
    
    private void Awake()
    {
        _enemyBase = transform.parent.GetComponent<EnemyBase>();
        _getDamageCollider = GetComponent<Collider>();
        currentMaxHealth = _maxHealth;
        _currentHealth = currentMaxHealth;
    }

    public bool IsPlayer() => _isPlayer;

    public void TakeDamage(float damage, DamageNumber visualEffect)
    {
        if (_enemyBase.IsDead)
        {
            return;
        }
        _currentHealth -= damage;

        visualEffect.Spawn(transform.position + new Vector3(0f, 2f, 0f), (int)damage);

        _enemyBase.EnemyFSM.Trigger(Enums.StateEvent.HitEnemy);
        hud.UpdateHealthBar(_currentHealth, currentMaxHealth);
        
        if (_currentHealth <= 0)
        {
            _getDamageCollider.enabled = false;
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
        _getDamageCollider.enabled = true;
        _currentHealth = currentMaxHealth;
        hud.UpdateHealthBar(_currentHealth, currentMaxHealth);
    }

}
