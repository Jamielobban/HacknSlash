using MoreMountains.Feedbacks;
using UnityEngine;
using UnityHFSM;

public class RangedAttack : BaseEnemyAttack
{
    public GameObject bulletPrefab, channelPrefab;
    public MMFeedbacks attackSound;
    public Transform instantiatePoint;
    public Transform instantiateChannelPoint;
    private GameObject channellingPrefab;
    public void OnShoot(State<Enums.EnemyStates, Enums.StateEvent> state)
    {
        _enemyBase.transform.LookAt(_enemyBase.target.transform.position);
        Use();
    }

    protected override void SetVisualEffects()
    {
        base.SetVisualEffects();
        //Audio
    }

    public void ShootAction()
    {
        GameObject bullet = Instantiate(bulletPrefab, instantiatePoint.position, Quaternion.identity);
        bullet.GetComponent<ProjectileMover>().damage = _currentDamage;
        bullet.GetComponent<ProjectileMover>().Shoot();
        bullet.GetComponent<ProjectileMover>().getDamage = _enemyBase.playerGetDamageNumber;
        attackSound.PlayFeedbacks();    
    }

    public void EnableChannelAction()
    {
        channellingPrefab = Instantiate(channelPrefab, instantiatePoint.position, Quaternion.identity);
    }

    public void DisableChannelAction()
    {
        if(channellingPrefab != null)
            Destroy(channellingPrefab);
    }
}
