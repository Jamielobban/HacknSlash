using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : Enemy
{

    protected override void Start()
    {
        events.OnIdle += () => SetState(gameObject.AddComponent<IdleState>());
        events.OnPatrolling += () => SetState(gameObject.AddComponent<PatrollState>());
        events.OnAttacking += () => SetState(gameObject.AddComponent<TreeAttackState>());
        events.OnFollowing += () => SetState(gameObject.AddComponent<ChaseState>());
        events.OnAir += () => SetState(gameObject.AddComponent<AirState>());
        events.OnHit += () => { SetState(gameObject.AddComponent<HitState>()); };
        events.OnDie += () => { SetState(gameObject.AddComponent<DeadState>()); isDead = true; sound.PlaySoundDead(); };

        ResetEnemy();
    }
}
