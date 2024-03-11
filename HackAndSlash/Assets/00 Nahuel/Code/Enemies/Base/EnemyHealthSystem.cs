using DamageNumbersPro;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageableEnemy
{
    protected Enemy _enemy;
    protected float _currentHealth;
    public CharacterStat baseMaxHealth = new CharacterStat();
    public float currentMaxHealth;
    public DamageNumber normalPrefab;
    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    private PlayerControl _player;
    public void ResetHealthEnemy()
    {
        _currentHealth = currentMaxHealth;
        ChangeLife();
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
        if(_enemy.isDead)
        {
            return;
        }

        _currentHealth -= damage;

        if(_enemy.hitsEffects.Count != 0)
        {
            switch (state)
            {
                case PlayerControl.HitState.DEBIL:
                    _enemy.currentHitEffect = _enemy.hitsEffects[0];
                    _enemy.currentFeedbacksEffect = _enemy.hitsFeedbacks[0];
                    break;
                case PlayerControl.HitState.MEDIO:
                    _enemy.currentHitEffect = _enemy.hitsEffects[1];
                    _enemy.currentFeedbacksEffect = _enemy.hitsFeedbacks[1];

                    break;
                case PlayerControl.HitState.FUERTE:
                    _enemy.currentHitEffect = _enemy.hitsEffects[2];
                    _enemy.currentFeedbacksEffect = _enemy.hitsFeedbacks[2];
                    break;
                default:
                    break;
            }
            _enemy.events.Hit();
        }

        DamageEffects(damage);
        if(_currentHealth <= 0)
        {
            _enemy.events.Die();
        }
    }
    private void DamageEffects(float dmg)
    {
        //*** Text Effects ***//
        if((int)dmg != 0)
        {
            normalPrefab.Spawn(transform.position + new Vector3(0f, 2f, 0f), (int)dmg);
        }
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
