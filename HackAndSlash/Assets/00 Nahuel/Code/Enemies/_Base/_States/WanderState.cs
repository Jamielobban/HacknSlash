using UnityEngine;
using UnityEngine.AI;

public class WanderState : EnemyStateBase
{
    private Transform _target;
    public WanderState(bool needsExitTime, Enemy enemy) : base(needsExitTime, enemy) { }

    public override void OnEnter()
    {
        base.OnEnter();
        _agent.enabled = true;
        _agent.isStopped = false;
        _animator.Play("Wander State");
        _target.position = GetRandomPosition();
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!_requestedExit)
        {
            _agent.SetDestination(_target.position);
        }
        else if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _target.position = GetRandomPosition();

            // //If we request exit, we will continue move to last known pos prior to idle
            // fsm.StateCanExit();
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
