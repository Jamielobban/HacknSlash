using UnityEngine;

public class SpawnState : EnemyStateBase
{
    public SpawnState(bool needsExitTime, EnemyBase enemyBase) : base(needsExitTime, enemyBase) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _animator.CrossFadeInFixedTime("Idle", 0.2f);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!_enemyBase._spawnEffect.IsSpawning)
        {
            fsm.StateCanExit();
        }
    }
}
