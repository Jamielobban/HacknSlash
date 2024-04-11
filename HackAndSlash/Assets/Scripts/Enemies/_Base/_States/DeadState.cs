using UnityEngine;
public class DeadState : EnemyStateBase
{
    public DeadState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        base.OnEnter();
        
        _animator.Play("Dead State");
        
        WaitExtensioNonMonobehavior.Wait(_animator.GetCurrentAnimatorClipInfo(0).Length + 1f, () =>
        {
            _enemy.gameObject.SetActive(false);
        });
    }
}
