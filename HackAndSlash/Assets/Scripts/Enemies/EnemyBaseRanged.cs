﻿using UnityEngine;
using UnityHFSM;
public class EnemyBaseRanged : EnemyBase
{
    [SerializeField] protected EnemySensor _shootAttackSensor;
    [SerializeField] protected RangedAttack _rangedAttack;
    
    protected override void Start()
    {
        base.Start();
        _shootAttackSensor.OnPlayerEnter += BasicAttackSensor_OnPlayerEnter;
        _shootAttackSensor.OnPlayerExit += BasicAttackSensor_OnPlayerExit;
    }
    
        protected override void InitializeStates()
    {
        base.InitializeStates();
        _enemyFSM.AddState(Enums.EnemyStates.Attack, new AttackState(false, this, _rangedAttack.OnShoot));
    }

    protected override void InitializeTriggerTransitions()
    {
        base.InitializeTriggerTransitions();
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Hit));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.SpawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Spawning));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DespawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Spawning));
    }

    protected override void InitializeHitTransitions()
    {
        base.InitializeHitTransitions();
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Attack, ShouldRangeAttack));
    }

    protected override void InitializeDeadTransitions()
    {
        base.InitializeDeadTransitions();
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Dead, (transition) => IsDead)); 
    }

    protected override void InitializeAttackTransitons()
    {
        base.InitializeAttackTransitons();
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Attack, ShouldRangeAttack,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Attack, ShouldRangeAttack,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
    }
    protected virtual bool ShouldRangeAttack(Transition<Enums.EnemyStates> transition) => !IsHit && _rangedAttack.CurrentAttackState == Enums.AttackState.ReadyToUse && _isInBasicRange;
    protected virtual void BasicAttackSensor_OnPlayerEnter(Transform player) => _isInBasicRange = true;
    protected virtual void BasicAttackSensor_OnPlayerExit(Vector3 lastKnownPosition) => _isInBasicRange = false;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _shootAttackSensor.OnPlayerEnter -= BasicAttackSensor_OnPlayerEnter;
        _shootAttackSensor.OnPlayerExit -= BasicAttackSensor_OnPlayerExit;
    }
}
