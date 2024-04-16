using UnityEngine;
using UnityHFSM;
public class AutoDestructionAttack : AreaAttack
{
    protected override void AttackAction()
    {
        GameObject go = Instantiate(areaPrefab, pointToInstantiate.position, Quaternion.identity);
        go.transform.GetChild(0).GetComponent<DamageDealer>().damage = _currentDamage;
    }

    protected override void OnAnimationEnd()
    {
        base.OnAnimationEnd();
        _enemyBase.IsDead = true;
        _enemyBase.OnDespawnEnemy();
    }
}
