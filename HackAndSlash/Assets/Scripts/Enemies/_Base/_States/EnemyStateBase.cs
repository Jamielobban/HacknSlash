using UnityHFSM;
using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyStateBase : State<Enums.EnemyStates, Enums.StateEvent>
{
    protected readonly EnemyBase EnemyBase;
    protected readonly NavMeshAgent _agent;
    protected readonly Animator _animator;
    
    protected bool _requestedExit;
    protected float _exitTime;
    
    protected readonly Action<State<Enums.EnemyStates, Enums.StateEvent>> onEnter;
    protected readonly Action<State<Enums.EnemyStates, Enums.StateEvent>> onLogic;
    protected readonly Action<State<Enums.EnemyStates, Enums.StateEvent>> onExit;
    protected readonly Func<State<Enums.EnemyStates, Enums.StateEvent>, bool> canExit;
    
    public EnemyStateBase(bool needsExitTime, EnemyBase enemyBase, float ExitTime = 0.1f,
        Action<State<Enums.EnemyStates, Enums.StateEvent>> onEnter = null, Action<State<Enums.EnemyStates, Enums.StateEvent>> onLogic = null,
        Action<State<Enums.EnemyStates, Enums.StateEvent>> onExit = null, Func<State<Enums.EnemyStates, Enums.StateEvent>, bool> canExit = null)
    {
        this.EnemyBase = enemyBase;
        this.onEnter = onEnter;
        this.onLogic = onLogic;
        this.onExit = onExit;
        this.canExit = canExit;
        this._exitTime = ExitTime;
        this.needsExitTime = needsExitTime;
        _agent = enemyBase.GetComponent<NavMeshAgent>();
        _animator = enemyBase.GetComponent<Animator>();
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _requestedExit = false;
        onEnter?.Invoke(this);
    }

    public override void OnLogic()
    {
        base.OnLogic();
        if (_requestedExit && timer.Elapsed >= _exitTime)
        {
            fsm.StateCanExit();
        }
    }

    public override void OnExitRequest()
    {
        if (!needsExitTime || canExit != null && canExit(this))
        {
            fsm.StateCanExit();
        }
        else
        {
            _requestedExit = true;
        }
    }
}
