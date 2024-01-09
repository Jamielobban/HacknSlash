using UnityEngine;

public class RangeAttack : EnemyBaseAttack
{
    public GameObject prefabProjectile;
    public Transform shootPoint;

    protected override void AttackAction()
    {
        GameObject go = Instantiate(prefabProjectile);
        go.transform.position = shootPoint.position;
        go.GetComponent<EnemiesProjetile>().damage = data.damage.Value;
    }
}
