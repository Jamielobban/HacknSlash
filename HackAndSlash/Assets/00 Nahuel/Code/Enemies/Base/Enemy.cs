using DamageNumbersPro.Demo;
using UnityEngine;

// -- Base Class for each enemy -- //
public class Enemy : PoolableObject
{
    #region Enemy Scripts References
    public EnemyEvents events { get; private set; }
    public EnemyHud hud { get; private set; }
    public EnemyMovement movements { get; private set; }
    public EnemyAnimations animations { get; private set; }
    public EnemyStats stats { get; private set; }
    public EnemyAttackHolder attackHolder { get; private set; }
    public EnemyHealthSystem healthSystem { get; private set; }

    #endregion
    public PlayerManager _player { get; private set; }
    public IState currentState;

    public GameObject lookAtEnemy;

    public bool isDead = false;
    public bool isGrounded = false;
    public bool canAttack = true;
    public bool onAir = false;

    protected virtual void Awake()
    {
        attackHolder = GetComponent<EnemyAttackHolder>();
        stats = GetComponent<EnemyStats>();
        healthSystem = transform.GetChild(1).GetComponent<EnemyHealthSystem>();
        animations = transform.GetChild(0).GetComponent<EnemyAnimations>();
        events = GetComponent<EnemyEvents>();
        hud = GetComponent<EnemyHud>();
        movements = GetComponent<EnemyMovement>();
        _player = FindObjectOfType<PlayerManager>();
    }

    protected virtual void Start()
    {
        // Configurar eventos de transici�n
        events.OnIdle += () => SetState(gameObject.AddComponent<IdleState>());
        events.OnPatrolling += () => SetState(gameObject.AddComponent<PatrollState>());
        events.OnAttacking += () => SetState(gameObject.AddComponent<AttackState>());
        events.OnFollowing += () => SetState(gameObject.AddComponent<ChaseState>());
        events.OnAir += () => SetState(gameObject.AddComponent<AirState>());
        events.OnHit += () => SetState(gameObject.AddComponent<ChaseState>());
        events.OnDie += () => { SetState(gameObject.AddComponent<DeadState>()); isDead = true; };

        ResetEnemy();
    }

    protected virtual void FixedUpdate()
    {
    }
    protected virtual void Update()
    {
        currentState?.UpdateState(this);
    }

    public virtual void ResetEnemy()
    {
        if(isPooleable)
        {
            UpgradeEnemy(RoomManager.Instance.GetScaleFactor());
        }
        healthSystem.ResetHealthEnemy();
        events.Idle();
    }

    private void UpgradeEnemy(float scaleFactor)
    {
        healthSystem.currentMaxHealth *= scaleFactor;
    }

    public virtual void SetState(IState newState)
    {
        if (currentState == newState)
        {
            return;
        }
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }
}
