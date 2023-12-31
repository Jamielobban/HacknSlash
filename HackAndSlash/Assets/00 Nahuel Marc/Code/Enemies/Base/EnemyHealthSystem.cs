using DamageNumbersPro;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageable
{
    protected Enemy _enemy;
    protected float _currentHealth;
    public CharacterStat maxHealth = new CharacterStat();

    #region Prefabs Effects

    public DamageNumber critPrefab, normalPrefab;
    public GameObject[] hitWhiteEffects;
    public GameObject[] bloodEffects;
    public GameObject hitEffect2;

    #endregion

    protected virtual void Awake()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
        _currentHealth = maxHealth.Value;
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
        if(_currentHealth > maxHealth.Value)
        {
            _currentHealth = maxHealth.Value;
        }
        _enemy.events.Heal();
    }

    public virtual void TakeDamage(float damage, bool isCrit, Vector3 collisionPoint)
    {
        _currentHealth -= damage;
        _enemy.events.Hit();
        DamageEffects(damage, isCrit, collisionPoint);
        if(_currentHealth <= 0)
        {
            _enemy.events.Die();
            Destroy(gameObject); // Destroy the collider so he won't be hit again after death
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
        _enemy.events.Air();
    }

    private void ChangeLife()
    {
        _enemy.hud.UpdateHealthBar(_currentHealth, maxHealth.Value);
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
