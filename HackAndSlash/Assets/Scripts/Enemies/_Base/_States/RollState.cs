﻿using System;
using UnityEngine;
using UnityHFSM;

public class RollState : EnemyStateBase
{
    public RollState(bool needsExitTime, EnemyBase enemyBase, Action<State<Enums.EnemyStates, Enums.StateEvent>> onEnter, float exitTime) : base (needsExitTime, enemyBase, exitTime, onEnter) {}
    private float _timer;
    private Vector3 direction;
    public override void OnEnter()
    {
        _agent.isStopped = true;
        base.OnEnter();
        direction = (_enemyBase.target.transform.position - _enemyBase.transform.position).normalized;
    }

    public override void OnLogic()
    {
        _timer += Time.deltaTime;
        base.OnLogic();


        if (_timer <= 0.75f)
        {
            Vector3 dashMove = direction * 1.5f * (_agent.speed * Time.deltaTime);
            _agent.Move(dashMove);
        }
        else
        {
            Vector3 moveAmount = direction *  _agent.speed * Time.deltaTime;
            _agent.Move(moveAmount);
        }

        //if (_timer <= 0.75f)
        //{
        //    Vector3 dashMove = (_enemyBase.target.transform.position - _enemyBase.transform.position).normalized * 2f * (_agent.speed * Time.deltaTime);
        //    _agent.Move(dashMove);
        //}
        //else if (_timer < 2 && _timer > 0.75f)
        //{
        //    // Perform dash movement
        //    Vector3 dashMovement = _agent.transform.forward.normalized * 2f * (_agent.speed * Time.deltaTime);
        //    _agent.Move(dashMovement);
        //}
        if(_timer > 2)
        {
            _agent.isStopped = false;
            _enemyBase.isAttacking = false;
            _timer = 0;
            fsm.StateCanExit();
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        _enemyBase.isAttacking = false;
        _timer = 0;
    }
}
