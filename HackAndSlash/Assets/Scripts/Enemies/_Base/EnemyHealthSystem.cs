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

    public Debuff[] debuffs;
    
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

        // AUDIO GET DAMAGE

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

    public void ApplyPoison()
    {
        float dmg = MaxHealth * 0.03f; // 5% of max health
        debuffs[0].ApplyDebuff(dmg);
    }

    public void ApplyBleed()
    {
        float dmg = MaxHealth * 0.04f; // 20% of dmg base atk
        debuffs[1].ApplyDebuff(dmg);
    }

    public void ApplyBurn()
    {
        float dmg = MaxHealth * 0.05f; // 40 % of damage base

        debuffs[2].ApplyDebuff(dmg);
    }
}
