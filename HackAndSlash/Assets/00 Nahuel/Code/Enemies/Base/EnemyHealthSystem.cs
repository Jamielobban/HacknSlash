using DamageNumbersPro;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageableEnemy
{
    protected Enemy _enemy;
    protected float _currentHealth;
    public CharacterStat baseMaxHealth = new CharacterStat();
    public float currentMaxHealth;
    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    #region Prefabs Effects
    private PlayerControl _player;

    #endregion
    public void ResetHealthEnemy()
    {
        _currentHealth = currentMaxHealth;
        ChangeLife();
        gameObject.SetActive(true);
    }
    protected virtual void Awake()
    {
        _player = FindObjectOfType<PlayerControl>();
        _enemy = transform.parent.GetComponent<Enemy>();
        currentMaxHealth = baseMaxHealth.Value;

        _currentHealth = currentMaxHealth;
    }

    protected virtual void Start()
    {
        _enemy.events.OnHeal += ChangeLife;
        //AbilityPowerManager.instance.IncreaseCombo();
        _enemy.events.OnHit += () => { ChangeLife();  };
    }

    public virtual void Stun(float stunTime)
    {
        _enemy.events.OnStun += () => _enemy.SetState(new StunState(stunTime));
        _enemy.events.Stun();
    }

    public virtual void Heal(float amount)
    {
        _currentHealth += amount;
        if(_currentHealth > currentMaxHealth)
        {
            _currentHealth = currentMaxHealth;
        }
        _enemy.events.Heal();
    }

    //  bool isCrit, Vector3 collisionPoint
    public virtual void TakeDamage(PlayerControl.HitState state,float damage)
    {

        _currentHealth -= damage;
        //_player.CallItemOnHit();
        _enemy.events.Hit();
       // DamageEffects(damage, isCrit, collisionPoint);
        if(_currentHealth <= 0)
        {
            _enemy.events.Die();
            gameObject.SetActive(false);
        }
    }
    private void DamageEffects(float dmg, bool critical, Vector3 collisionPoint)
    {
        //*** Text Effects ***//

      //  DamageNumber damageNumber = critical ? critPrefab.Spawn(collisionPoint + new Vector3(0f, 1.5f, 0f), (int)dmg) : normalPrefab.Spawn(collisionPoint + new Vector3(0f, 1.5f, 0f), (int)dmg);

        //*** Particle Effects ***//

        //SpawnWhiteSplash(collisionPoint);
        //SpawnBloodSplash(collisionPoint);
        //SpawnHitLine(collisionPoint);
    }
    public virtual void AirDamageable()
    {
        if(!_enemy.onAir)
        {
            _enemy.events.Air();
        }
    }

    private void ChangeLife()
    {
        _enemy.hud.UpdateHealthBar(_currentHealth, currentMaxHealth);
    }
}
