using Cinemachine;
using DamageNumbersPro;
using MoreMountains.Feedbacks;
using System;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityHFSM;

[RequireComponent(typeof(Animator),typeof(NavMeshAgent))]
public class EnemyBase : PoolableObject
{
    #region Settings
    [Header("References")] 
    [SerializeField] protected PlayerControl _player;
    [SerializeField] protected EnemyHealthSystem _healthSystem;
    public EnemySpawnEffect _spawnEffect { get; private set; }
    
    [Header("Sensors")] 
    [SerializeField] protected EnemySensor _followPlayerSensor;
    
    [Space] [Header("Base Attacks")] 
    public List<BaseEnemyAttack> attacks = new List<BaseEnemyAttack>();

    [Header("Variables")]
    public DamageNumber playerGetDamageNumber;
    public Transform target;
    public Vector3 targetToSpecialMove;
    public GameObject healthBar;
    public GameObject spawner;
    public float experience;
    public float timeBetweenAttacks = 1.5f;
    public bool affectedBySpecialMove = false;
    public float exitTimeAttacks = 1f;
    public MMFeedbacks deadSound;
    public MMFeedbacks spawnSound;
    public float predictionTime = .1f;

    public Collider getEnemyCollider;
    
    public float _currentTime = 0f;
    protected Animator _animator;
    protected NavMeshAgent _agent;
    protected StateMachine<Enums.EnemyStates, Enums.StateEvent> _enemyFSM;
    #endregion
    
    [Header("Debug Info")]
    protected bool _isInFollowRange;
    protected bool _isInBasicRange;
    protected bool _enableMeleeAttack = false;
    public bool IsHit = false;
    public bool IsDead = false;
    public bool attackInterrumpted = false;
    public bool isAttacking = false;
    public bool isStun = false;
    public bool attackCanBeInterrupted = false;

    public StructureEnemiesLife nearStructure = null; // for check if it's near a structure that will stack enemy deaths

    // Stadistics //
    public float baseMoveSpeed;
    public float timeStuned;

    // -- Getters -- //
    public PlayerControl Player => _player;
    public NavMeshAgent Agent => _agent;
    public StateMachine<Enums.EnemyStates, Enums.StateEvent> EnemyFSM => _enemyFSM;
    public EnemyHealthSystem HealthSystem => _healthSystem;

    protected virtual void Awake()
    {

        _player = GameManager.Instance.Player;
        _healthSystem = transform.GetChild(0).GetComponent<EnemyHealthSystem>();
        _spawnEffect = GetComponent<EnemySpawnEffect>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyFSM = new StateMachine<Enums.EnemyStates, Enums.StateEvent>();
        getEnemyCollider = GetComponent<Collider>();
        baseMoveSpeed = Agent.speed;
        
        InitializeStates();
        InitializeTransitions();

        if (target == null)
        {
            target = _player.transform;
        }
    }

    protected virtual void Start()
    {
        _followPlayerSensor.OnPlayerEnter += FollowPlayerSensor_OnPlayerSensorEnter;
        _followPlayerSensor.OnPlayerExit += FollowPlayerSensor_OnPlayerSensorExit;    
        _healthSystem.GetDamageCollider.enabled = false;
        healthBar.SetActive(false);
    }

    protected virtual void InitializeStates()
    {
        // Base States
        _enemyFSM.AddState(Enums.EnemyStates.Idle, new IdleState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Chase, new ChaseState(false, this, target));
        _enemyFSM.AddState(Enums.EnemyStates.Hit, new HitState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Stun, new StunState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Dead, new DeadState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Spawning, new SpawnState(false, this));
        //_enemyFSM.AddState(Enums.EnemyStates.Countering, new CounteringState(false, this));
        //_enemyFSM.AddState(Enums.EnemyStates.Wander, new WanderState(false, this));
    }
    
    #region Initialize Transitions
    protected virtual void InitializeTransitions()
    {
        InitializeTriggerTransitions();
        InitializeHitTransitions();
        InitializeDeadTransitions();

        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Spawning, Enums.EnemyStates.Idle, (transition) => !_spawnEffect.IsSpawning));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
        
        InitializeAttackTransitons();
    }
    
    protected virtual void InitializeTriggerTransitions()
    {
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DetectPlayer, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Chase));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.LostPlayer, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Idle));
        
        // Hit Transitions
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Hit));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.HitEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Hit));
        
        // Spawning Transitions
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.SpawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Spawning));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.SpawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Spawning));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.SpawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Spawning));
        
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DespawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Spawning));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DespawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Spawning));
        _enemyFSM.AddTriggerTransition(Enums.StateEvent.DespawnEnemy, new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Spawning));
    }
    protected virtual void InitializeHitTransitions()
    {
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Idle, (transition) => IsInIdleRange()));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Hit, Enums.EnemyStates.Chase, (transition) => InRangeToChase()));
    }
    protected virtual void InitializeDeadTransitions()
    {
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Idle, Enums.EnemyStates.Dead, (transition) => IsDead));
        _enemyFSM.AddTransition(new Transition<Enums.EnemyStates>(Enums.EnemyStates.Chase, Enums.EnemyStates.Dead, (transition) => IsDead));
    }
    protected virtual void InitializeAttackTransitons() { }
    #endregion
    
    protected virtual bool IsInIdleRange() => (!_isInFollowRange || Vector3.Distance(target.position, transform.position) <= _agent.stoppingDistance) && !IsHit && !isAttacking;
    protected virtual bool InRangeToChase() => _isInFollowRange && Vector3.Distance(target.position, transform.position) > _agent.stoppingDistance && !IsHit && !isAttacking;

    protected virtual bool CanAttackBeInterrupted(Transition<Enums.EnemyStates> transition) => !attackCanBeInterrupted;

    protected virtual void Update()
    {
        _currentTime += Time.deltaTime;
        _enemyFSM.OnLogic();
        DespawnByDistance();
        SetMeleeAttackEnable();

    }

    protected void DespawnByDistance()
    {
        if (Time.frameCount % 30 == 0 && gameObject.activeSelf)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, _player.transform.position)) >= 80)
            {
                if (isPooleable)
                {
                    if (spawner != null)
                    {
                        SpawnerBase _baseSpawner = spawner.GetComponent<SpawnerBase>();
                        InfiniteSpawner _baseInfinity = spawner.GetComponent<InfiniteSpawner>();
                        if (_baseSpawner != null)
                        {
                            _baseSpawner.RemoveEnemy(this);
                        }
                        else if (_baseInfinity != null)
                        {
                            _baseInfinity.RemoveEnemy(this);
                        }
                    }
                    ResetEnemy();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    protected void SetMeleeAttackEnable()
    {
        if (_currentTime >= timeBetweenAttacks && !_enableMeleeAttack)
        {
            _enableMeleeAttack = true;
        }
        else if (_currentTime <= timeBetweenAttacks && _enableMeleeAttack)
        {
            _enableMeleeAttack = false;
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

    public void OnSpawnEnemy()
    {
        if (target == null)
        {
            target = Player.transform;
        }

        getEnemyCollider.enabled = true;
        _enemyFSM.SetStartState(Enums.EnemyStates.Spawning);
        _enemyFSM.Init();
        _enemyFSM.Trigger(Enums.StateEvent.SpawnEnemy);
        ResetEnemy();

        spawnSound.PlayFeedbacks();

        gameObject.SetActive(true);
        _spawnEffect.InitializeSpawnEffect();
    }

    public void OnDespawnEnemy()
    {
        getEnemyCollider.enabled = false;
        _enemyFSM.Trigger(Enums.StateEvent.DespawnEnemy);
        deadSound.PlayFeedbacks();
        if (nearStructure != null && !nearStructure.isActive)
        {
            nearStructure.AddKilledEnemy();
            GameObject go = Instantiate(nearStructure.particleToCharge,
                new Vector3(transform.position.x, transform.position.y + .75f, transform.position.z), Quaternion.identity);
            go.GetComponent<SimpleProjectileSphere>().direction = nearStructure.transform;
        }
        _player.experienceManager.GainXp(experience);
        _spawnEffect.InitializeDespawnEffect();
    }
    
    public virtual void ResetEnemy()
    {
        IsDead = false;
        _spawnEffect.ResetDissolveAmount();
        _healthSystem.ResetEnemyHealth();
    }

    public virtual void OnDie()
    {        
        attackInterrumpted = false;
        if (isPooleable)
        {
            if (spawner != null)
            {
                if (spawner.GetComponent<SpawnerBase>())
                {
                    spawner.GetComponent<SpawnerBase>().RemoveEnemy(this);
                }
                else if (spawner.GetComponent<InfiniteSpawner>())
                {
                    spawner.GetComponent<InfiniteSpawner>().RemoveEnemy(this);
                }
                LevelManager.Instance.EnemiesManager.AddSpawnedEnemies(-1);
            }
            spawner = null;
            ResetEnemy();
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
        healthBar.SetActive(false);

    }
    
    public void UpgradeEnemy(float scaleFactorHp, float scaleFactorDmg)
    {
        float lastHealth = _healthSystem.currentMaxHealth;
        float newHealth = _healthSystem.MaxHealth * scaleFactorHp;
        _healthSystem.currentMaxHealth = newHealth;
        
        if (attacks.Count > 0)
        {
            foreach (BaseEnemyAttack attack in attacks)
            {
                float newDamage= attack.baseDamage * scaleFactorDmg;
                attack.SetCurrentDamage(newDamage);
            }
        }

        if (gameObject.activeSelf)
        {
            if (_healthSystem.CurrentHealth >= lastHealth)
            {
                _healthSystem.ResetEnemyHealth();
            }
        }
        else
        {
            ResetEnemy();
        }
    }
    
    protected virtual void OnDestroy()
    {
        _followPlayerSensor.OnPlayerEnter -= FollowPlayerSensor_OnPlayerSensorEnter;
        _followPlayerSensor.OnPlayerExit -= FollowPlayerSensor_OnPlayerSensorExit;      
    }
    
}
