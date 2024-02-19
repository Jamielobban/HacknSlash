using System.Collections;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    protected PlayerManager _player;
    public DataAttack data;
    public Enums.AttackState currentAttackState = Enums.AttackState.ReadyToUse;
    private float _currentTime = 0f;
    public Vector3 instantiationPoint;
    public Quaternion initialRotation;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        if(currentAttackState == Enums.AttackState.Cooldown)
        {
            _currentTime -= Time.deltaTime;
            if (!IsInCD())
            {
                currentAttackState = Enums.AttackState.ReadyToUse;
            }
        }
    }

    public bool IsInCD() => _currentTime < data.cooldown && _currentTime >= 0;

    public void ResetAttack()
    {
        _currentTime = -1;
        currentAttackState = Enums.AttackState.ReadyToUse;
    }

    public void Use()
    {
        if(currentAttackState != Enums.AttackState.ReadyToUse)
        {
            return;
        }
        if(!CharacterIsOnAllowedState())
        {
            return;
        }
        if(!IsInCD())
        {
            StartCoroutine(HandleAttack());
        }
    }

    private IEnumerator HandleAttack()
    {
        currentAttackState = Enums.AttackState.Casting;
        _player.animations.PlayTargetAnimation(data.animation, true);
        AddSoundEffects();
        yield return new WaitForSeconds(data.castTime);
        SetVisualEffects();
        _currentTime = data.cooldown;
        currentAttackState = Enums.AttackState.Cooldown;
    }

    protected virtual void AddSoundEffects()
    {

    }
    protected virtual void SetVisualEffects()
    {

    }
    private bool CharacterIsOnAllowedState()
    {
        return data.AllowedCharacterStates.Contains(_player.CurrentCharacterState);
    }
}
