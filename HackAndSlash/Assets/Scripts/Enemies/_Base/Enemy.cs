using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityHFSM;

[RequireComponent(typeof(Animator),typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] protected PlayerControl _player;
    
    [Header("Sensors")] 
    [SerializeField] protected EnemySensor _followPlayerSensor;
    [SerializeField] protected EnemySensor _meleeAttackSensor;
    
    [Header("Debug Info")]
    [SerializeField] protected bool _isInFollowRange;
    [SerializeField] protected bool _isInBasicRange;
    public bool IsHit = false;

    [Space] [Header("Base Attacks")] 
    [SerializeField] protected MeleeAttack _meleeAttack;
    
    protected Animator _animator;
    protected NavMeshAgent _agent;
    protected StateMachine<Enums.EnemyStates, Enums.StateEvent> _enemyFSM;
    public StateMachine<Enums.EnemyStates, Enums.StateEvent> EnemyFSM => _enemyFSM;

    public Transform target;
    protected virtual void Awake()
    {
        _player = GameManager.Instance.Player;
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyFSM = new StateMachine<Enums.EnemyStates, Enums.StateEvent>();
        
        InitializeStates();
        InitializeTransitions();

        _enemyFSM.SetStartState(Enums.EnemyStates.Idle);
        _enemyFSM.Init();

        if (target == null)
        {
            target = _player.transform;
        }
    }

    protected virtual void Start()
    {
        _followPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerSensorEnter;
        _followPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerSensorExit;        
        _meleeAttackSensor.OnPlayerEnter += BasicAttackSensor_OnPlayerEnter;
        _meleeAttackSensor.OnPlayerExit += BasicAttackSensor_OnPlayerExit;
    }

    protected virtual void InitializeStates()
    {
        // Base States
        _enemyFSM.AddState(Enums.EnemyStates.Idle, new IdleState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Chase, new ChaseState(false, this, target));
        _enemyFSM.AddState(Enums.EnemyStates.Hit, new HitState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Stun, new StunState(false, this, 2));
        _enemyFSM.AddState(Enums.EnemyStates.Attack, new AttackState(false, this, _meleeAttack.OnAttack));
        _enemyFSM.AddState(Enums.EnemyStates.Dead, new DeadState(false, this));
        //_enemyFSM.AddState(Enums.EnemyStates.Countering, new CounteringState(false, this));
        //_enemyFSM.AddState(Enums.EnemyStates.Wander, new WanderState(false, this));
    }

    protected virtual void InitializeTransitions()
    {
        InitializeTriggerTransitions();
        InitializeHitTransitions();
        
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
        
        InitializeAttackTransitons();
    }
    
    protected virtual void InitializeTriggerTransitions()
    {
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DetectPlayer, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Chase));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.LostPlayer, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Idle));
        
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Hit));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Hit));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Hit));
        
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DeadEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Dead));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DeadEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Dead));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DeadEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Dead));
    }
    protected virtual void InitializeHitTransitions()
    {
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Attack, ShouldMeleeAttack));
    }
    protected virtual void InitializeAttackTransitons()
    {
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Attack, ShouldMeleeAttack,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Attack, ShouldMeleeAttack,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
    }
    
    protected virtual bool ShouldMeleeAttack(Transition<Enums.EnemyStates> transition) => !IsHit && _meleeAttack.CurrentAttackState == Enums.AttackState.ReadyToUse && _isInBasicRange;
    protected virtual bool IsInIdleRange() => (!_isInFollowRange || Vector3.Distance(target.position, transform.position) <= _agent.stoppingDistance) && !IsHit ;
    protected virtual bool InRangeToChase() => _isInFollowRange && Vector3.Distance(target.position, transform.position) > _agent.stoppingDistance && !IsHit;
    protected virtual void Update()
    {
        _enemyFSM.OnLogic();
    }

    protected virtual void FollowPlayerSensor_OnPlayerSensorEnter(Transform player)
    {
        _enemyFSM.Trigger(Enums.StateEvent.DetectPlayer);
        _isInFollowRange = true;  
    }
    protected virtual void FollowPlayerSensor_OnPlayerSensorExit(Vector3 lastKnownPosition)
    {
        _enemyFSM.Trigger(Enums.StateEvent.LostPlayer);
        _isInFollowRange = false;
    } 
    protected virtual void BasicAttackSensor_OnPlayerEnter(Transform player) => _isInBasicRange = true;
    protected virtual void BasicAttackSensor_OnPlayerExit(Vector3 lastKnownPosition) => _isInBasicRange = false;
    
    protected virtual void OnDestroy()
    {
        _followPlayerSensor.OnPlayerEnter -= FollowPlayerSensor_OnPlayerSensorEnter;
        _followPlayerSensor.OnPlayerExit -= FollowPlayerSensor_OnPlayerSensorExit;        
        _meleeAttackSensor.OnPlayerEnter -= BasicAttackSensor_OnPlayerEnter;
        _meleeAttackSensor.OnPlayerExit -= BasicAttackSensor_OnPlayerExit;
    }
}
