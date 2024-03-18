public class HitState : EnemyStateBase
{
    public HitState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        base.OnEnter();
        _animator.Play("Hit");
    }
}
