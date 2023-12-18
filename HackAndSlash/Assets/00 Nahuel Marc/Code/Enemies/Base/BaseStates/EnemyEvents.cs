using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    public event Action OnIdle;
    public event Action OnAttacking;
    public event Action OnPatrolling;
    public event Action OnFollowing;

    public event Action OnHeal;
    public event Action OnDie;
    public event Action OnHit;
    public event Action OnStun;
    public event Action OnAir;

    #region InvokeActions
    public void Heal() => OnHeal?.Invoke();
    public void Idle() => OnIdle?.Invoke();
    public void Patrolling() => OnPatrolling?.Invoke();
    public void Following() => OnFollowing?.Invoke();
    public void Attacking() => OnAttacking?.Invoke();
    public void Hit() => OnHit?.Invoke();
    public void Air() => OnAir?.Invoke();
    public void Stun() => OnStun?.Invoke();
    public void Die() => OnDie?.Invoke();
    #endregion

}
