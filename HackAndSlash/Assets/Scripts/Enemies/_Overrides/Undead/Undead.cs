using UnityEngine;
using UnityEngine.Serialization;
using UnityHFSM;

public class Undead : EnemyBaseMeleeCamp
{
         [FormerlySerializedAs("_areaAttack")]
         [Header("Custom Attacks")] 
    [SerializeField] protected MeleeAttack _doubleHitAttack;
    [SerializeField] protected EnemySensor _doubleHitSensor;

    protected bool _inRangeToArea;
    
    protected override void Start()
    {
        base.Start();
        _doubleHitSensor.OnPlayerEnter += DoubleHitAttack_SensorOnPlayerEnter;
        _doubleHitSensor.OnPlayerExit += DoubleHitAttack_SensorOnPlayerExit;
    }
    
    protected override void InitializeStates()
    {
        base.InitializeStates();
        _enemyFSM.AddState(Enums.EnemyStates.AreaAttack, new AttackState(false, this, _doubleHitAttack.OnAttack));
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
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Hit));
    }
    protected virtual void InitializeAreaTransitions()
    {
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.AreaAttack, ShouldArea, null, null,true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.AreaAttack, ShouldArea,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.AreaAttack, ShouldArea,null, null, true));

        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
    }
    
    protected virtual bool ShouldArea(Transition<Enums.EnemyStates> transition) => !IsHit && _doubleHitAttack.CurrentAttackState == Enums.AttackState.ReadyToUse && _inRangeToArea && !isAttacking;

    protected virtual void DoubleHitAttack_SensorOnPlayerEnter(Transform player) => _inRangeToArea = true;
    protected virtual void DoubleHitAttack_SensorOnPlayerExit(Vector3 lastKnownPosition) => _inRangeToArea = false;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _doubleHitSensor.OnPlayerEnter -= DoubleHitAttack_SensorOnPlayerEnter;
        _doubleHitSensor.OnPlayerExit -= DoubleHitAttack_SensorOnPlayerExit;
    }

        
}
