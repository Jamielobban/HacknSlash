using UnityEngine;
using UnityHFSM;
public class SpiderNormal : EnemyBaseMelee
{
    public override void OnDie()
    {
        if (isPooleable)
        {
            spawner = null;
            ResetEnemy();
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
