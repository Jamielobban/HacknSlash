using UnityEngine;
public class StunState : EnemyStateBase
{
    private float _stunTime;
    private float _currentTime;
    public StunState(bool needsExitTime, Enemy enemy, float stunTime) : base(needsExitTime, enemy, stunTime)
    {
        _stunTime = stunTime;
    }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        base.OnEnter();
        _animator.CrossFade("Stun", 0.2f);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        _currentTime += Time.deltaTime;
        if (_currentTime >= _stunTime)
        {
            fsm.StateCanExit();
        }
    }
}
