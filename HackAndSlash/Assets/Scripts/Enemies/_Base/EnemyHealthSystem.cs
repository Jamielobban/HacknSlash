using DamageNumbersPro;
using MoreMountains.Feedbacks;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    public MMFeedbacks onHit;
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

        visualEffect.Spawn(transform.position + new Vector3(0f, Random.Range(1.8f, 2.2f), 0f), (int)damage);


        onHit.PlayFeedbacks();

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

    public void ApplyBurn()
    {
        float dmg = MaxHealth * 0.05f; // 40 % of damage base

        debuffs[0].ApplyDebuff(dmg);
    }

    public void ApplyStun(float timeStun)
    {
        _enemyBase.timeStuned = timeStun;
        _enemyBase.isStun = true;
    }
}
