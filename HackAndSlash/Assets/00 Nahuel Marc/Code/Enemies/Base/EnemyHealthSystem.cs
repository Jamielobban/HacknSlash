using DamageNumbersPro;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageable
{
    protected Enemy _enemy;
    protected float _currentHealth;
    public CharacterStat baseMaxHealth = new CharacterStat();
    public float currentMaxHealth;

    #region Prefabs Effects
    private PlayerControl _player;
    public DamageNumber critPrefab, normalPrefab;
    public GameObject[] hitWhiteEffects;
    public GameObject[] bloodEffects;
    public GameObject hitEffect2;

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
        _enemy.events.OnHit += () => { ChangeLife(); AbilityPowerManager.instance.IncreaseCombo(); };
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

    public virtual void TakeDamage(float damage, bool isCrit, Vector3 collisionPoint)
    {
        _currentHealth -= damage;
        _player.CallItemOnHit();
        _enemy.events.Hit();
        DamageEffects(damage, isCrit, collisionPoint);
        if(_currentHealth <= 0)
        {
            _enemy.events.Die();
            gameObject.SetActive(false);
        }
    }
    private void DamageEffects(float dmg, bool critical, Vector3 collisionPoint)
    {
        //*** Text Effects ***//

        DamageNumber damageNumber = critical ? critPrefab.Spawn(collisionPoint + new Vector3(0f, 1.5f, 0f), (int)dmg) : normalPrefab.Spawn(collisionPoint + new Vector3(0f, 1.5f, 0f), (int)dmg);

        //*** Particle Effects ***//

        SpawnWhiteSplash(collisionPoint);
        SpawnBloodSplash(collisionPoint);
        SpawnHitLine(collisionPoint);
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

    private void SpawnEffect(GameObject effect, Vector3 spawnPoint)
    {
        GameObject playerCenter = GameObject.FindGameObjectWithTag("PlayerCenter");
        GameObject hitToLook = Instantiate(effect, spawnPoint, Quaternion.identity);
        hitToLook.transform.LookAt(playerCenter.transform.position);
    }

    private void SpawnWhiteSplash(Vector3 spawnPoint)
    {
        SpawnEffect(hitWhiteEffects[Random.Range(0, hitWhiteEffects.Length - 1)], spawnPoint + new Vector3(0f, 1f, 0f));
    }

    private void SpawnBloodSplash(Vector3 spawnPoint)
    {
        SpawnEffect(bloodEffects[Random.Range(0, bloodEffects.Length)], spawnPoint + new Vector3(0f, 1f, 0f));
    }

    private void SpawnHitLine(Vector3 spawnPoint)
    {
        SpawnEffect(hitEffect2, spawnPoint + new Vector3(Random.Range(0, 0.15f), Random.Range(0.85f, 1.15f), 0f));
    }
}
