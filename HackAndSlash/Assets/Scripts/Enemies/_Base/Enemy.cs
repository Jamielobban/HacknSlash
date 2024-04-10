using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityHFSM;

[RequireComponent(typeof(Animator),typeof(NavMeshAgent))]
public class Enemy : PoolableObject
{
    #region Settings
    [Header("References")] 
    [SerializeField] protected PlayerControl _player;
    [SerializeField] protected EnemyHealthSystem _healthSystem;
    
    [Header("Sensors")] 
    [SerializeField] protected EnemySensor _followPlayerSensor;
    [SerializeField] protected EnemySensor _meleeAttackSensor;
    
    [Space] [Header("Base Attacks")] 
    public List<BaseEnemyAttack> attacks = new List<BaseEnemyAttack>();
    [SerializeField] protected MeleeAttack _meleeAttack;
    
    [Header("Variables")]
    public Transform target;
    public GameObject spawner;
    
    protected Animator _animator;
    protected NavMeshAgent _agent;
    protected StateMachine<Enums.EnemyStates, Enums.StateEvent> _enemyFSM;
    #endregion
    
    [Header("Debug Info")]
    [SerializeField] protected bool _isInFollowRange;
    [SerializeField] protected bool _isInBasicRange;
    public bool IsHit = false;
    public bool IsDead = false;
    
    // -- Getters -- //
    public PlayerControl Player => _player;
    public NavMeshAgent Agent => _agent;
    public StateMachine<Enums.EnemyStates, Enums.StateEvent> EnemyFSM => _enemyFSM;

    protected virtual void Awake()
    {
        _player = GameManager.Instance.Player;
        _healthSystem = transform.GetChild(0).GetComponent<EnemyHealthSystem>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyFSM = new StateMachine<Enums.EnemyStates, Enums.StateEvent>();
        
        InitializeStates();
        InitializeTransitions();
        OnSpawnEnemy();

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
        
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Dead, (transition) => IsDead));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Dead, (transition) => IsDead));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Attack, Enums.EnemyStates.Dead, (transition) => IsDead));
        
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
        // If enemy is to far despawn
        if(Time.frameCount % 30 == 0 && gameObject.activeSelf)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, _player.transform.position)) >= 80)
            {
                if (isPooleable)
                {
                    if (spawner.GetComponent<SpawnerBase>())
                    {
                        spawner.GetComponent<SpawnerBase>().RemoveEnemy(this);
                    }
                    else
                    {
                        ManagerEnemies.Instance.SetSpawnedEnemies(-1);
                    }
                    gameObject.SetActive(false);
                }
            }
        }
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

    public void OnSpawnEnemy()
    {
        ResetEnemy();
        // SetActive true
        // Shader To Spawn
        _enemyFSM.SetStartState(Enums.EnemyStates.Idle);
        _enemyFSM.Init();
    }

    public void OnDespawnEnemy()
    {
        // Shader To Despawn
        // Deactive GameObject if it's pooleable // or Destroy if else
    }
    
    public virtual void ResetEnemy()
    {
        IsDead = false;
        _healthSystem.ResetEnemyHealth();
    }
    
    public virtual void UpgradeEnemy(float scaleFactorHp, float scaleFactorDmg)
    {
        float newHealth = _healthSystem.MaxHealth * scaleFactorHp;
        _healthSystem.currentMaxHealth = newHealth;

        if (_healthSystem.CurrentHealth >= _healthSystem.currentMaxHealth)
        {
            ResetEnemy();
        }
        
        if (attacks.Count > 0)
        {
            foreach (BaseEnemyAttack attack in attacks)
            {
                float newDamage= attack.baseDamage * scaleFactorDmg;
                attack.SetCurrentDamage(newDamage);
            }
        }

        // if (gameObject.activeSelf)
        // {
        //
        // }
    }
    
    protected virtual void OnDestroy()
    {
        _followPlayerSensor.OnPlayerEnter -= FollowPlayerSensor_OnPlayerSensorEnter;
        _followPlayerSensor.OnPlayerExit -= FollowPlayerSensor_OnPlayerSensorExit;        
        _meleeAttackSensor.OnPlayerEnter -= BasicAttackSensor_OnPlayerEnter;
        _meleeAttackSensor.OnPlayerExit -= BasicAttackSensor_OnPlayerExit;
    }
}
