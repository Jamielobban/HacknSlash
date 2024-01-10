using UnityEngine;

// -- Base Class for each enemy -- //
public class Enemy : PoolableObject
{
    public IState currentState;
    public EnemyEvents events { get; private set; }
    public EnemyHud hud { get; private set; }
    public EnemyMovement movements { get; private set; }
    public EnemyAnimations animations { get; private set; }
    public EnemyStats stats { get; private set; }
    public EnemyAttackHolder attackHolder { get; private set; }
    public EnemyHealthSystem healthSystem { get; private set; }

    public bool isDead = false;
    public bool isGrounded = false;
    public bool canAttack = true;
    public bool onAir = false;
    public virtual void SetState(IState newState)
    {
        if(currentState == newState)
        {
            return;
        }
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    protected virtual void Awake()
    {
        attackHolder = GetComponent<EnemyAttackHolder>();
        stats = GetComponent<EnemyStats>();
        healthSystem = transform.GetChild(1).GetComponent<EnemyHealthSystem>();
        animations = transform.GetChild(0).GetComponent<EnemyAnimations>();
        events = GetComponent<EnemyEvents>();
        hud = GetComponent<EnemyHud>();
        movements = GetComponent<EnemyMovement>();
    }

    protected virtual void Start()
    {
        // Configurar eventos de transici�n
        events.OnIdle += () => SetState(gameObject.AddComponent<IdleState>());
        events.OnPatrolling += () => SetState(gameObject.AddComponent<PatrollState>());
        events.OnAttacking += () => SetState(gameObject.AddComponent<AttackState>());
        events.OnFollowing += () => SetState(gameObject.AddComponent<ChaseState>());
        //events.OnStun += () => SetState(new StunState(0f));
        events.OnAir += () => SetState(gameObject.AddComponent<AirState>());
        events.OnDie += () => { SetState(gameObject.AddComponent<DeadState>()); isDead = true; };

        events.Idle();
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
        healthSystem.ResetHealthEnemy();
        events.Idle();
    }
}
