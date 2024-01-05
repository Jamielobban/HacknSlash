using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseAttack : MonoBehaviour
{
    public EnemyAttackData data;
    public Enums.EnemyAttackState currentAttackState = Enums.EnemyAttackState.ReadyToUse;
    protected float _currentTime;
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        if(currentAttackState == Enums.EnemyAttackState.Cooldown)
        {
            _currentTime -= Time.deltaTime;
            if(!IsInCd())
            {
                ResetAbility();
            }
        }
    }

    public virtual bool IsInCd()
    {
        return _currentTime < data.cooldown.Value && _currentTime >= 0;
    }

    public virtual void ResetAbility()
    {
        currentAttackState = Enums.EnemyAttackState.ReadyToUse;
    }

    public virtual void Use()
    {
        if(currentAttackState != Enums.EnemyAttackState.ReadyToUse)
        {
            return;
        }
        if(!IsInCd())
        {
            StartCoroutine(HandleAttack());
        }
    }

    private IEnumerator HandleAttack()
    {
        currentAttackState = Enums.EnemyAttackState.Casting;
        yield return new WaitForSeconds(data.castTime.Value);
        SetVisualEffects();
        _currentTime = data.cooldown.Value;
        currentAttackState = Enums.EnemyAttackState.Cooldown;
    }
    protected virtual void SetVisualEffects()
    {

    }
}
