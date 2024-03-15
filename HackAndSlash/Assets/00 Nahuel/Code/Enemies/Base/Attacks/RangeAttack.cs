using UnityEngine;

public class RangeAttack : EnemyBaseAttack
{
    public GameObject prefabProjectile;
    public Transform shootPoint;
    public float ableDistToShoot = 2f;
    public override bool IsInRangeToAttack(float dist)
    {
        return dist <= data.range.Value && dist >= ableDistToShoot;
    }
    protected override void AttackAction()
    {
        GameObject go = Instantiate(prefabProjectile);
        go.transform.position = shootPoint.position;
        go.GetComponent<EnemiesProjetile>().damage = data.damage.Value;
    }
}
