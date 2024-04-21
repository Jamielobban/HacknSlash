using UnityEngine;
using UnityHFSM;
public class Necromancer : EnemyBaseRanged
{
    [SerializeField] protected MeleeAttack _meleeAttack;
    [SerializeField] protected InvokeAttack _invokeAttack;

    protected override void InitializeStates()
    {
        base.InitializeStates();
        _enemyFSM.AddState(Enums.EnemyStates.Attack, new AttackState(false, this, _meleeAttack.OnAttack));
        _enemyFSM.AddState(Enums.EnemyStates.Invoking, new AttackState(false, this, _invokeAttack.OnInvoke));
    }

    protected override void InitializeTriggerTransitions()
    {
        base.InitializeTriggerTransitions();
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Hit));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.SpawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Spawning));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DespawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Spawning));
        
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.SpawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Invoking, Enums.EnemyStates.Spawning));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DespawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Invoking, Enums.EnemyStates.Spawning));
    }

    protected override void InitializeHitTransitions()
    {
        base.InitializeHitTransitions();
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Attack, ShouldMeleeAttack));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Invoking, ShouldInvoke));
    }

    protected override void InitializeDeadTransitions()
    {
        base.InitializeDeadTransitions();
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Dead, (transition) => IsDead)); 
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Invoking, Enums.EnemyStates.Dead, (transition) => IsDead)); 
    }

    protected override void InitializeAttackTransitons()
    {
        base.InitializeAttackTransitons();
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Attack, ShouldMeleeAttack,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Attack, ShouldMeleeAttack,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Ranged, Enums.EnemyStates.Attack, ShouldMeleeAttack,null, null, true));
        
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
        
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Invoking, ShouldInvoke,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Invoking, ShouldInvoke,null, null, true));
        
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Invoking, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Invoking, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
    }
    protected virtual bool ShouldMeleeAttack(Transition<Enums.EnemyStates> transition) => !IsHit && _rangedAttack.CurrentAttackState == Enums.AttackState.ReadyToUse && 
        Vector3.Distance(transform.position, _player.transform.position) <= 3 && _enableMeleeAttack && !isAttacking;

    protected virtual bool ShouldInvoke(Transition<Enums.EnemyStates> transition) =>
        !IsHit && _invokeAttack.CurrentAttackState == Enums.AttackState.ReadyToUse && !isAttacking;

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
