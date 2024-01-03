using UnityEngine;

// -- Base Class for each enemy -- //
public class Enemy : MonoBehaviour
{
    public IState currentState;
    public EnemyEvents events { get; private set; }
    public EnemyHud hud { get; private set; }
    public EnemyMovement movements { get; private set; }
    public EnemyAnimations animations { get; private set; }
    public EnemyStats stats { get; private set; }

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
        stats = GetComponent<EnemyStats>();
        animations = transform.GetChild(0).GetComponent<EnemyAnimations>();
        events = GetComponent<EnemyEvents>();
        hud = GetComponent<EnemyHud>();
        movements = GetComponent<EnemyMovement>();
    }

    protected virtual void Start()
    {
        // Configurar eventos de transici�n
        events.OnIdle += () => SetState(new IdleState());
        events.OnPatrolling += () => SetState(new PatrollState());
        events.OnAttacking += () => SetState(new AttackState());
        events.OnFollowing += () => SetState(new ChaseState());
        //events.OnStun += () => SetState(new StunState(0f));
        events.OnAir += () => SetState(new AirState());

        events.Idle();
    }
    protected virtual void FixedUpdate()
    {

    }
    protected virtual void Update()
    {
        currentState?.UpdateState(this);
    }
}
