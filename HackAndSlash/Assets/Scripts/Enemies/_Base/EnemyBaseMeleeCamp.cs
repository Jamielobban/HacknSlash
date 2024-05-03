using UnityEngine;
public class EnemyBaseMeleeCamp : EnemyBaseMelee
{
    public CampManager campManager;
    public Transform initialPoint;
    public float toFarDistance;

    protected override void Start()
    {
        base.Start();
        OnSpawnEnemy();
    }

    protected override void Update()
    {
        base.Update();
        
        if (Vector3.Distance(initialPoint.position, transform.position) > toFarDistance)
        {
            transform.position = initialPoint.position;
        }
    }
    public override void OnDie()
    {
        campManager.RemoveEnemy(gameObject);
        Destroy(gameObject);
    }
}
