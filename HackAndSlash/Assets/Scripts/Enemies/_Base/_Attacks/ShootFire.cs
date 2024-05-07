using MoreMountains.Feedbacks;
using UnityEngine;
using UnityHFSM;

public class ShootFire : BaseEnemyAttack
{
    public GameObject[] bulletPrefab;
    public MMFeedbacks attackSound;
    public void OnShoot(State<Enums.EnemyStates, Enums.StateEvent> state)
    {
        _enemyBase.transform.LookAt(_enemyBase.target.transform.position);
        Use();
    }

    protected override void SetVisualEffects()
    {
        base.SetVisualEffects();
    }

    public void ShootAction()
    {
        for (int i = 0; i < bulletPrefab.Length; i++)
        {
            if (bulletPrefab[i].GetComponent<FlameThrower>() != null)
            {
                bulletPrefab[i].SetActive(true);
                bulletPrefab[i].GetComponent<FlameThrower>().damage = _currentDamage;
            }
        }
        attackSound.PlayFeedbacks();
    }
}
