using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class BaseAbility : MonoBehaviour
{
    public AbilityData data;

    public Enums.AttackState currentAbilityState = Enums.AttackState.ReadyToUse;
    public Image hudCooldown;
    protected float _currentTime;
    protected float _currentCooldown;
    protected float _currentCastTime;
    protected float _currentDamage;
    protected PlayerControl _player;
    protected virtual void Awake()
    {
        _player = FindObjectOfType<PlayerControl>();
        InitializeVariables();
    }
    protected virtual void Start() { }

    protected void InitializeVariables()
    {
        _currentCooldown = data.baseCooldown;
        _currentCastTime = data.baseCastTime;
        _currentDamage = data.baseDamage;
    }
    

    protected virtual void Update()
    {
        if (currentAbilityState == Enums.AttackState.Cooldown)
        {
            _currentTime -= Time.deltaTime;
            UpdateHud();
            if(!IsInCD())
            {
                hudCooldown.fillAmount = 0;
                currentAbilityState = Enums.AttackState.ReadyToUse;
            }
        }
    }
  
    public virtual void ResetAbility()
    {
        _currentTime = -1;
        hudCooldown.fillAmount = 0;
        currentAbilityState = Enums.AttackState.ReadyToUse;
    }

    public virtual void Use()
    {
        if (currentAbilityState != Enums.AttackState.ReadyToUse)
        {
            return;
        }
        if(!IsInCD())
        {
            StartCoroutine(HandleAbility());
        }
    }
    
    private IEnumerator HandleAbility()
    {
        currentAbilityState = Enums.AttackState.Casting;
        _player.playerAnim.CrossFadeInFixedTime(data.animation, 0.2f);
        AddVoiceEffect();
        yield return new WaitForSeconds(_currentCastTime);
        SetVisualEffect();
        _currentTime = _currentCooldown;
        currentAbilityState = Enums.AttackState.Cooldown;
    }

    public virtual bool IsInCD() => _currentTime < _currentCooldown && _currentTime >= 0;
    private void UpdateHud() => hudCooldown.fillAmount = Mathf.Clamp(_currentTime / _currentCooldown, 0, 1);
    protected virtual void SetVisualEffect() { }
    protected virtual void AddVoiceEffect() { }

    
}


