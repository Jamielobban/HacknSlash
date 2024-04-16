using UnityEngine;
using UnityEngine.Serialization;
using UnityHFSM;

public class SpiderExplosive : EnemyBase
{
    [Header("Custom Attacks")] 
    [SerializeField] protected AutoDestructionAttack _autoDestructionAttack;
    [SerializeField] protected EnemySensor _autoDestructionSensor;

    protected bool _inRangeToArea;
    
    protected override void Start()
    {
        base.Start();
        _autoDestructionSensor.OnPlayerEnter += AutoDestructionAttackSensor_OnPlayerEnter;
        _autoDestructionSensor.OnPlayerExit += AutoDestructionAttackSensor_OnPlayerExit;
    }
    
    protected override void InitializeStates()
    {
        base.InitializeStates();
        _enemyFSM.AddState(Enums.EnemyStates.AreaAttack, new AttackState(false, this, _autoDestructionAttack.OnAreaAttack));
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
    }
    protected virtual void InitializeAreaTransitions()
    {
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.AreaAttack, ShouldArea, null, null,true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.AreaAttack, ShouldArea,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.AreaAttack, ShouldArea,null, null, true));

        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
    }
    
    protected virtual bool ShouldArea(Transition<Enums.EnemyStates> transition) => !IsHit && _autoDestructionAttack.CurrentAttackState == Enums.AttackState.ReadyToUse && _inRangeToArea && !isAttacking;

    protected virtual void AutoDestructionAttackSensor_OnPlayerEnter(Transform player) => _inRangeToArea = true;
    protected virtual void AutoDestructionAttackSensor_OnPlayerExit(Vector3 lastKnownPosition) => _inRangeToArea = false;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _autoDestructionSensor.OnPlayerEnter -= AutoDestructionAttackSensor_OnPlayerEnter;
        _autoDestructionSensor.OnPlayerExit -= AutoDestructionAttackSensor_OnPlayerExit;
    }
    
    
    
    
    public override void OnDie()
    {
        if (isPooleable)
        {
            spawner = null;
            ResetEnemy();
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
