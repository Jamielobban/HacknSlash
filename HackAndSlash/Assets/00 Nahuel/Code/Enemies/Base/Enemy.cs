using DamageNumbersPro.Demo;
using MoreMountains.Feedbacks;
using System.Collections.Generic;
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
    public PlayerControl _player { get; private set; }
    public IState currentState;

    public List<OnHitEffect> hitsEffects = new List<OnHitEffect>();
    public List<MMFeedbacks> hitsFeedbacks = new List<MMFeedbacks>();

    public OnHitEffect currentHitEffect;
    public MMFeedbacks currentFeedbacksEffect;

    public GameObject lookAtEnemy;

    public bool isDead = false;
    public bool isGrounded = false;
    public bool canAttack = true;
    public bool onAir = false;
    public bool attackInterrupted = false;

    protected virtual void Awake()
    {
        attackHolder = GetComponent<EnemyAttackHolder>();
        stats = GetComponent<EnemyStats>();
        healthSystem = transform.GetChild(1).GetComponent<EnemyHealthSystem>();
        animations = transform.GetChild(0).GetComponent<EnemyAnimations>();
        events = GetComponent<EnemyEvents>();
        hud = GetComponent<EnemyHud>();
        movements = GetComponent<EnemyMovement>();
        _player = FindObjectOfType<PlayerControl>();
    }

    protected virtual void Start()
    {
        events.OnIdle += () => SetState(gameObject.AddComponent<IdleState>());
        events.OnPatrolling += () => SetState(gameObject.AddComponent<PatrollState>());
        events.OnAttacking += () => SetState(gameObject.AddComponent<AttackState>());
        events.OnFollowing += () => SetState(gameObject.AddComponent<ChaseState>());
        events.OnAir += () => SetState(gameObject.AddComponent<AirState>());
        events.OnHit += () => { SetState(gameObject.AddComponent<HitState>()); };
        events.OnDie += () => { SetState(gameObject.AddComponent<DeadState>()); isDead = true; };

        ResetEnemy();
    }

    protected virtual void FixedUpdate()
    {
    }
    protected virtual void Update()
    {
        currentState?.UpdateState(this);
        if (Time.frameCount % 10 == 0)
        {
            if(Mathf.Abs(Vector3.Distance(transform.position, movements.target.position)) >= 120)
            {
                if (isPooleable)
                {
                    ManagerEnemies.Instance.SetSpawnedEnemies(-1);
                    ResetEnemy();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public virtual void ResetEnemy()
    {
        if(isPooleable)
        {
           // UpgradeEnemy(RoomManager.Instance.GetScaleFactor());
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
