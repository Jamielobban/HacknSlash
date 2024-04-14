using UnityEngine;
using UnityHFSM;
public class Necromancer : EnemyBaseRanged
{
    [SerializeField] protected EnemySensor _meleeAttackSensor;
    [SerializeField] protected MeleeAttack _meleeAttack;

    private bool _isInMeleeRanged = false;
    
    protected override void Start()
    {
        base.Start();
        _meleeAttackSensor.OnPlayerEnter += MeleeAttackSensor_OnPlayerEnter;
        _meleeAttackSensor.OnPlayerExit += MeleeAttackSensor_OnPlayerExit;
    }
    
        protected override void InitializeStates()
    {
        base.InitializeStates();
        _enemyFSM.AddState(Enums.EnemyStates.Attack, new AttackState(false, this, _meleeAttack.OnAttack));
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
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Attack, ShouldMeleettack));
    }

    protected override void InitializeDeadTransitions()
    {
        base.InitializeDeadTransitions();
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Dead, (transition) => IsDead)); 
    }

    protected override void InitializeAttackTransitons()
    {
        base.InitializeAttackTransitons();
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Attack, ShouldMeleettack,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Attack, ShouldMeleettack,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
    }
    protected virtual bool ShouldMeleettack(Transition<Enums.EnemyStates> transition) => !IsHit && _rangedAttack.CurrentAttackState == Enums.AttackState.ReadyToUse && _isInMeleeRanged && _enableMeleeAttack && !isAttacking;
    protected virtual void MeleeAttackSensor_OnPlayerEnter(Transform player) => _isInMeleeRanged = true;
    protected virtual void MeleeAttackSensor_OnPlayerExit(Vector3 lastKnownPosition) => _isInMeleeRanged = false;

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _shootAttackSensor.OnPlayerEnter -= MeleeAttackSensor_OnPlayerEnter;
        _shootAttackSensor.OnPlayerExit -= MeleeAttackSensor_OnPlayerExit;
    }
}
