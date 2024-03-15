using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : Enemy
{

    public new EnemyEventsSpider events;

    protected override void Awake()
    {
        base.Awake();
        events = GetComponent<EnemyEventsSpider>();
    }
    public override void SetState(IState newState)
    {
        // Configurar eventos de transicion
        events.OnIdle += () => SetState(gameObject.AddComponent<IdleState>());
        events.OnPatrolling += () => SetState(gameObject.AddComponent<PatrollState>());
        events.OnAttacking += () => SetState(gameObject.AddComponent<AttackState>());
        events.OnFollowing += () => SetState(gameObject.AddComponent<ChaseState>());
        events.OnAir += () => SetState(gameObject.AddComponent<AirState>());
        events.OnHit += () => SetState(gameObject.AddComponent<HitState>());
        events.OnDie += () => { SetState(gameObject.AddComponent<DeadState>()); isDead = true; sound.PlaySoundDead(); };
        ResetEnemy();
    }
}
