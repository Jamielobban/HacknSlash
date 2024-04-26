using UnityEngine;
using UnityHFSM;

[RequireComponent(typeof(Collider))]
public class AreaAttack : BaseEnemyAttack
{
    public GameObject areaPrefab;
    public Transform pointToInstantiate;
    private Collider _colliderDamage;
    protected override void Awake()
    {
        base.Awake();
        _colliderDamage = GetComponent<Collider>();
        _colliderDamage.enabled = false;
    }
    public void OnAreaAttack(State<Enums.EnemyStates, Enums.StateEvent> state)
    {
        _enemyBase.transform.LookAt(_enemyBase.target.transform.position);
        _enemyBase._currentTime = 0f;
        Use();
    }
    protected override void AttackAction()
    {
        base.AttackAction();
        GameObject go = Instantiate(areaPrefab, pointToInstantiate.position, Quaternion.identity);
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (!_enemyBase.attackInterrumpted)
        {
            if (_enemyBase.target == _enemyBase.Player.transform && damageable.IsPlayer())
            {
                damageable.TakeDamage(_currentDamage, _enemyBase.playerGetDamageNumber);
            }
            else if (_enemyBase.target != _enemyBase.Player.transform && !damageable.IsPlayer())
            {
                damageable.TakeDamage(_currentDamage, _enemyBase.playerGetDamageNumber);
            }
        }
        else
        {
            _enemyBase.attackInterrumpted = false;
        }
    }
}
