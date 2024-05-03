using UnityEngine;
public class StunState : EnemyStateBase
{
    private float _currentTime;
    public StunState(bool needsExitTime, EnemyBase enemyBase) : base(needsExitTime, enemyBase)
    {
    }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        base.OnEnter();
        _animator.CrossFadeInFixedTime("Stun", 0.2f);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        _currentTime += Time.deltaTime;
        if (_currentTime >= _enemyBase.timeStuned)
        {
            fsm.StateCanExit();
        }
    }
}
