using UnityEngine;
using UnityHFSM;

public class RangedAttack : BaseEnemyAttack
{
    public GameObject bulletPrefab;
    
    public void OnShoot(State<Enums.EnemyStates, Enums.StateEvent> state)
    {
        EnemyBase.transform.LookAt(EnemyBase.Player.transform.position);
        Use();
    }
    protected override void AttackAction()
    {
        base.AttackAction();
        //Spawn Bullet direction to player
    }
}
