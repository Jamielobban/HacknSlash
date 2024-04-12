﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class BaseEnemyAttack : MonoBehaviour
{
    protected EnemyBase EnemyBase;
    [SerializeField] protected Enums.AttackState _currentAttackState = Enums.AttackState.ReadyToUse;
    public Enums.AttackState CurrentAttackState => _currentAttackState;
    protected float _currentTime;
    protected Animator _animator;
    protected float _currentDamage;
 
    [Header("Enemy Stats:")] 
    public float baseDamage;
    public float minCooldown, maxCooldown;
    [SerializeField] protected float _castTime;
    [SerializeField] protected string _animationName; 
    
    protected float _cooldown;
    
    protected virtual void Awake()
    {
        _animator = transform.parent.parent.GetComponent<Animator>();
        _currentDamage = baseDamage;
        EnemyBase = transform.parent.parent.GetComponent<EnemyBase>();
        _cooldown = Random.Range(minCooldown, maxCooldown);
        _currentTime = 0f;
        EnemyBase.attackInterrumpted = false;
    }

    protected virtual void Update()
    {
        if(_currentAttackState == Enums.AttackState.Cooldown)
        {
            _currentTime -= Time.deltaTime;
            if(!IsInCd())
            {
                _currentAttackState = Enums.AttackState.ReadyToUse;
            }
        }
    }

    protected virtual void Use()
    {
        StartCoroutine(HandleAttack());
    }
    protected IEnumerator HandleAttack()
    {
        _currentAttackState = Enums.AttackState.Casting;
        PlayAttackAnimation();
        yield return new WaitForSeconds(_castTime);
        SetVisualEffects();
        AttackAction();
        //enemy.attackInterrupted = false;
        _currentTime = _cooldown;
        _currentAttackState = Enums.AttackState.Cooldown;
    }

    protected virtual void PlayAttackAnimation()
    {
        _animator.CrossFade(_animationName, 0.2f);
        this.Wait(_animator.GetCurrentAnimatorClipInfo(0).Length, () =>
        {
            //Animation Over
        });
    }
    protected virtual void SetVisualEffects() { }
    protected virtual void AttackAction() { }
    protected virtual bool IsInCd() => _currentTime < _cooldown && _currentTime >= 0;

    public void SetCurrentDamage(float value) => _currentDamage = value;
}
