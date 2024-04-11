using UnityEngine;

public class SpawnState : EnemyStateBase
{
    public SpawnState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _animator.CrossFade("Idle", 0.2f);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!_enemy._spawnEffect.IsSpawning)
        {
            fsm.StateCanExit();
        }
    }
}
