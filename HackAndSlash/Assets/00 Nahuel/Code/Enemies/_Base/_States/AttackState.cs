public class AttackState : EnemyStateBase
{
    public AttackState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        base.OnEnter();
        _animator.Play("Attack");
    }
}
