using UnityEngine;
using UnityHFSM;

public class Hydra : EnemyBaseMelee
{
    [Header("Custom Attacks")]
    [SerializeField] protected ShootFire _shootFire;
    [SerializeField] protected EnemySensor _sensorShoot;

    protected bool _inRangeToShoot;

    protected override void Start()
    {
        base.Start();
        _sensorShoot.OnPlayerEnter += ShootSensor_OnPlayerEnter;
        _sensorShoot.OnPlayerExit += ShootSensor_OnPlayerExit;
    }
    protected override void InitializeStates()
    {
        base.InitializeStates();
        _enemyFSM.AddState(Enums.EnemyStates.AreaAttack, new AttackState(false, this, _shootFire.OnShoot));
    }
    protected override void InitializeTransitions()
    {
        base.InitializeTransitions();
        InitializeAreaTransitions();
    }
    protected override void InitializeTriggerTransitions()
    {
        base.InitializeTriggerTransitions();
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DeadEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Dead));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Hit, CanAttackBeInterrupted));
    }
    protected virtual void InitializeAreaTransitions()
    {
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.AreaAttack, ShouldShoot, null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.AreaAttack, ShouldShoot, null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.AreaAttack, ShouldShoot, null, null, true));

        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.AreaAttack, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
    }

    protected virtual bool ShouldShoot(Transition<Enums.EnemyStates> transition) => !IsHit && _shootFire.CurrentAttackState == Enums.AttackState.ReadyToUse && _inRangeToShoot && !isAttacking;
    protected virtual void ShootSensor_OnPlayerEnter(Transform player) => _inRangeToShoot = true;
    protected virtual void ShootSensor_OnPlayerExit(Vector3 lastKnownPosition) => _inRangeToShoot = false;
    protected override void OnDestroy()
    {
        base.OnDestroy();
        _sensorShoot.OnPlayerEnter -= ShootSensor_OnPlayerEnter;
        _sensorShoot.OnPlayerExit -= ShootSensor_OnPlayerExit;
    }

}
