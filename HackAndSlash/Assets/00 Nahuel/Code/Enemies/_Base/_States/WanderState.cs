using UnityEngine;
using UnityEngine.AI;

public class WanderState : EnemyStateBase
{
    private Vector3 _target;
    public WanderState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _agent.enabled = true;
        _agent.isStopped = false;
        _animator.CrossFade("Wander State", 0.2f);
        _target = GetRandomPosition();
        _agent.SetDestination(_target);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _target = GetRandomPosition();
            _agent.SetDestination(_target);
        }
    }

    private Vector3 GetRandomPosition()
    {
        int count = 0;
        while (count< 5)
        { 
            Vector2 random = Random.insideUnitCircle * 5;
            Vector3 pos = _agent.transform.position + new Vector3(random.x, 0, random.y);
            if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                return hit.position;
            }

            count++;
        }

        return _agent.transform.position;
    }
}
