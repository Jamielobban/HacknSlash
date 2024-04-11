﻿using System;
using UnityEngine;
using UnityHFSM;

public class RollState : EnemyStateBase
{
    public RollState(bool needsExitTime, Enemy enemy, Action<State<Enums.EnemyStates, Enums.StateEvent>> onEnter, float exitTime) : base (needsExitTime, enemy, exitTime, onEnter) {}
    private float _timer;
    public override void OnEnter()
    {
        _agent.isStopped = true;
        base.OnEnter();
    }

    public override void OnLogic()
    {
        _timer += Time.deltaTime;
        base.OnLogic();
        if (_timer < 2)
        {
            // Perform dash movement
            Vector3 dashMovement = _agent.transform.forward.normalized * 2f * (_agent.speed * Time.deltaTime);
            _agent.Move(dashMovement);
        }
        else
        {
            _agent.isStopped = false;
            _enemy.isRolling = false;
            _timer = 0;
            fsm.StateCanExit();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        _enemy.isRolling = false;
        _timer = 0;
    }
}
