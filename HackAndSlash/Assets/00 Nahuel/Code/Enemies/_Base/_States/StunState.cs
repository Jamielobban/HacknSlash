public class StunState : EnemyStateBase
{
    private float _stunTime;

    public StunState(bool needsExitTime, Enemy enemy, float stunTime) : base(needsExitTime, enemy, stunTime)
    {
        _stunTime = stunTime;
    }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        base.OnEnter();
        _animator.Play("Stun");
    }
}
