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
        _enemyBase.transform.LookAt(_enemyBase.Player.transform.position);
        _enemyBase._currentTime = 0f;
        Use();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_enemyBase.attackInterrumpted)
        {
            other.GetComponent<IDamageable>().TakeDamage(_currentDamage);
        }
        else
        {
            _enemyBase.attackInterrumpted = false;
        }
    }
}
