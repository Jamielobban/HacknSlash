using Unity.VisualScripting;
using UnityEngine;

public class ChaseState : EnemyStateBase
{
    private bool _useMovementPrediction;
    public ChaseState(bool needsExitTime, EnemyBase enemyBase, Transform target) : base(needsExitTime, enemyBase)
    {
        _enemyBase.target = target;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _agent.enabled = true;
        _agent.isStopped = false;
        _animator.CrossFadeInFixedTime("Chase State", 0.2f);
        _useMovementPrediction = Random.value < 0.5f;
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (!_requestedExit)
        {
            if (_useMovementPrediction)
            {
                Vector3 destination = GameManager.Instance.Player.predictPoint.position;
                if (Vector3.Distance(_enemyBase.transform.position, GameManager.Instance.Player.transform.position) > 3f)
                {
                    _agent.SetDestination(destination);
                }
                else
                {
                    _agent.SetDestination(_enemyBase.target.position);
                }
            }
            else
            {
                _agent.SetDestination(_enemyBase.target.position);
            }

        }
        else if (_agent.remainingDistance <= _agent.stoppingDistance || _enemyBase.isAttacking)
        {
            //If we request exit, we will continue move to last known pos prior to idle
            fsm.StateCanExit();
        }
    }
}
