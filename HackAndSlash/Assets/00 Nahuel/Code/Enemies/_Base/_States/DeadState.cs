public class DeadState : EnemyStateBase
{
    public DeadState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        base.OnEnter();
        _animator.Play("die");
    }

    public override void OnLogic()
    {
        base.OnLogic();
    }
}
