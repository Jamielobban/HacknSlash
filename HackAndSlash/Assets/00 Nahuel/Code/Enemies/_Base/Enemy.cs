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

    [Space] [Header("Base Attacks")] 
    [SerializeField] protected MeleeAttack _meleeAttack;
    
    protected Animator _animator;
    protected NavMeshAgent _agent;
    protected StateMachine<Enums.EnemyStates, Enums.StateEvent> _enemyFSM;
    public StateMachine<Enums.EnemyStates, Enums.StateEvent> EnemyFSM => _enemyFSM;
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
        // Adding States
        _enemyFSM.AddState(Enums.EnemyStates.Idle, new IdleState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Chase, new ChaseState(false, this, _player.transform));
        _enemyFSM.AddState(Enums.EnemyStates.Wander, new WanderState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Hit, new HitState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Stun, new StunState(false, this, 2));
        _enemyFSM.AddState(Enums.EnemyStates.Attack, new AttackState(false, this, _meleeAttack.OnAttack));
        _enemyFSM.AddState(Enums.EnemyStates.Countering, new CounteringState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Dead, new DeadState(false, this));
    }

    protected virtual void InitializeTransitions()
    {
        // Adding Transitions
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DetectPlayer, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Chase));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.LostPlayer, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Idle));
        
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Chase, 
            (transition) => _isInFollowRange && Vector3.Distance(_player.transform.position, transform.position) > _agent.stoppingDistance));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Idle, 
            (transition) => !_isInFollowRange || Vector3.Distance(_player.transform.position, transform.position) <= _agent.stoppingDistance));
        
        // Adding Attack Transitions
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Attack, ShouldMeleeAttack,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Attack, ShouldMeleeAttack,null, null, true));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Chase, IsNotWithinIdleRange));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Idle, IsWithinIdleRange));
    }
    protected virtual bool ShouldMeleeAttack(Transition<Enums.EnemyStates> transition) => _meleeAttack.CurrentAttackState == Enums.AttackState.ReadyToUse && _isInBasicRange;
    protected virtual bool IsWithinIdleRange(Transition<Enums.EnemyStates> transition) => _agent.remainingDistance <= _agent.stoppingDistance;
    protected virtual bool IsNotWithinIdleRange(Transition<Enums.EnemyStates> transition) => !IsWithinIdleRange(transition);
    
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
