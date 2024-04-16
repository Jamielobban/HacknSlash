using UnityEngine;
using UnityHFSM;
public class AutoDestructionAttack : AreaAttack
{
    [SerializeField] protected EnemyHealthSystem _healthSystem;
    
    protected override void AttackAction()
    {
        GameObject go = Instantiate(areaPrefab, pointToInstantiate.position, Quaternion.identity);
        go.transform.GetChild(0).GetComponent<DamageDealer>().damage = _currentDamage;
    }

    protected override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        _healthSystem.TakeDamage(_healthSystem.CurrentHealth);
    }
}
