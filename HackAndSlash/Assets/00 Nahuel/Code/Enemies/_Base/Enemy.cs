﻿using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(Animator),typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    protected Animator _animator;
    protected NavMeshAgent _agent;
    protected StateMachine<Enums.EnemyStates, Enums.StateEvent> _enemyFSM;
    protected void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyFSM = new StateMachine<Enums.EnemyStates, Enums.StateEvent>();
        
        _enemyFSM.AddState(Enums.EnemyStates.Idle, new IdleState(false, this));
        //_enemyFSM.AddState(Enums.EnemyStates.Chase, new ChaseState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Wander, new WanderState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Hit, new HitState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Stun, new StunState(false, this, 2));
        _enemyFSM.AddState(Enums.EnemyStates.Attack, new AttackState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Countering, new CounteringState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Dead, new DeadState(false, this));
        
        _enemyFSM.SetStartState(Enums.EnemyStates.Idle);
        _enemyFSM.Init();
    }

    protected void Update()
    {
        _enemyFSM.OnLogic();
    }
}
