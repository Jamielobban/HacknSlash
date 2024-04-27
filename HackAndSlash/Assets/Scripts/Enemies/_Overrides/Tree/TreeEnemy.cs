using UnityHFSM;
using UnityEngine;

public class TreeEnemy : EnemyBaseMelee
{
    [Header("Custom Attacks")] 
    [SerializeField] protected AreaAttack _areaAttack;
    [SerializeField] protected EnemySensor _areaSensor;

    protected bool _inRangeToArea;
    
    protected override void Start()
    {
        base.Start();
        _areaSensor.OnPlayerEnter += AreaAttackSensor_OnPlayerEnter;
        _areaSensor.OnPlayerExit += AreaAttackSensor_OnPlayerExit;
    }
    
    protected override void InitializeStates()
    {
        base.InitializeStates();
        _enemyFSM.AddState(Enums.EnemyStates.AreaAttack, new AttackState(false, this, _areaAttack.OnAreaAttack));
    }
    protected override void InitializeTransitions()
    {
        base.InitializeTransitions();
        InitializeAreaTransitions();
    }
    
    protected override void InitializeTriggerTransitions()
    {
        base.InitializeTriggerTransitions();
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DeadEnemy,new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Dead));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Hit, CanAttackBeInterrupted));
    }
    protected virtual void InitializeAreaTransitions()
    {
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.AreaAttack, ShouldArea, null, null,true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.AreaAttack, ShouldArea,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.AreaAttack, ShouldArea,null, null, true));

        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
    }
    
    protected virtual bool ShouldArea(Transition<Enums.EnemyStates> transition) => !IsHit && _areaAttack.CurrentAttackState == Enums.AttackState.ReadyToUse && _inRangeToArea && !isAttacking;

    protected virtual void AreaAttackSensor_OnPlayerEnter(Transform player) => _inRangeToArea = true;
    protected virtual void AreaAttackSensor_OnPlayerExit(Vector3 lastKnownPosition) => _inRangeToArea = false;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _areaSensor.OnPlayerEnter -= AreaAttackSensor_OnPlayerEnter;
        _areaSensor.OnPlayerExit -= AreaAttackSensor_OnPlayerExit;
    }
}
