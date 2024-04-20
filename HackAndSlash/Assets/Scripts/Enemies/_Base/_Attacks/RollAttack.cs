using UnityEngine;
using UnityHFSM;

public class RollAttack : BaseEnemyAttack
{
    [SerializeField] private GameObject _sensor;
    private Collider _colliderDamage;
    public float rollDuration;
    protected override void Awake()
    {
        base.Awake();
        _colliderDamage = GetComponent<Collider>();
        _colliderDamage.enabled = false;
    }
    
    public void OnRoll()
    {
        _sensor.gameObject.SetActive(true);
        _enemyBase.transform.LookAt(_enemyBase.target.transform.position);
        _enemyBase._currentTime = 0f;
        Use();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (!_enemyBase.attackInterrumpted)
        {
            if (_enemyBase.target == _enemyBase.Player.transform && damageable.IsPlayer())
            {
                damageable.TakeDamage(_currentDamage);
            }
            else if (_enemyBase.target != _enemyBase.Player.transform && !damageable.IsPlayer())
            {
                damageable.TakeDamage(_currentDamage);
            }
        }
        else
        {
            _enemyBase.attackInterrumpted = false;
        }
    }
}
