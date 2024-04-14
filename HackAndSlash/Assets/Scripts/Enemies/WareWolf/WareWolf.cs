using UnityEngine;
using UnityHFSM;

public class WareWolf : EnemyBaseMelee
{
    [Header("Custom Attacks")]
    [SerializeField] protected RollAttack _roll;

    [SerializeField] protected ImpactSensor _rollImpactSensor;
    
    protected override void Start()
    {
        base.Start();
        _rollImpactSensor.OnCollision += RollImpactSensor_OnCollision;
    }

    protected override void InitializeStates()
    {
        base.InitializeStates();
        _enemyFSM.AddState(Enums.EnemyStates.Roll, new RollState(false, this, (onEnter) =>
        { _roll.OnRoll(); isAttacking = true; }, _roll.rollDuration));
    }

    protected override void InitializeTransitions()
    {
        base.InitializeTransitions();
        
        InitializeRollTransitions();
    }

    protected override void InitializeTriggerTransitions()
    {
        base.InitializeTriggerTransitions();
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.RollImpact,new Transition<Enums.EnemyStates>(Enums.EnemyStates.Roll, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.RollImpact,new Transition<Enums.EnemyStates>(Enums.EnemyStates.Roll, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DeadEnemy,new Transition<Enums.EnemyStates>(Enums.EnemyStates.Roll, Enums.EnemyStates.Dead));
      //  _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Roll, Enums.EnemyStates.Hit));
    }

    protected virtual void InitializeRollTransitions()
    {
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Roll, ShouldRoll, null, null,true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Roll, ShouldRoll,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Roll, ShouldRoll,null, null, true));

        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Roll, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Roll, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
    }

    protected override bool InRangeToChase() => base.InRangeToChase() && !isAttacking;
    protected override bool IsInIdleRange() => base.IsInIdleRange() && !isAttacking;

    protected virtual bool ShouldRoll(Transition<Enums.EnemyStates> transition) => !IsHit && _roll.CurrentAttackState == Enums.AttackState.ReadyToUse && _isInFollowRange &&
                                                                                   Vector3.Distance(_player.transform.position, transform.position) >= 4 && !isAttacking;
    
    protected virtual void RollImpactSensor_OnCollision(Collision collision)
    {
        _enemyFSM.Trigger(Enums.StateEvent.RollImpact);
        _rollImpactSensor.gameObject.SetActive(false);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _rollImpactSensor.OnCollision -= RollImpactSensor_OnCollision;
    }
}
