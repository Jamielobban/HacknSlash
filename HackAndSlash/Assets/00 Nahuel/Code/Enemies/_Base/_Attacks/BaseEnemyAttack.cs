using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class BaseEnemyAttack : MonoBehaviour
{
    protected PlayerControl _player;
    [Range(1, 20f)] [SerializeField] protected float _cooldown;
    [SerializeField] protected Enums.AttackState _currentAttackState = Enums.AttackState.ReadyToUse;
    [SerializeField] protected float _baseDamage;
    [SerializeField] protected float _currentDamage;
    [SerializeField] protected float _castTime;
    [SerializeField] protected string _animationName;
    protected float _currentTime;
    public Enums.AttackState CurrentAttackState => _currentAttackState;
    protected Animator _animator;
    protected virtual void Awake()
    {
        _animator = transform.parent.GetComponent<Animator>();
        _currentDamage = _baseDamage;
        _player = GameManager.Instance.Player;
        _currentTime = _cooldown;
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
        _animator.Play(_animationName);
        this.Wait(_animator.GetCurrentAnimatorClipInfo(0).Length, () =>
        {
            //Animation Over
        });
    }
    protected virtual void SetVisualEffects() { }
    protected virtual void AttackAction() { }
    protected virtual bool IsInCd() => _currentTime < _cooldown && _currentTime >= 0;
}
