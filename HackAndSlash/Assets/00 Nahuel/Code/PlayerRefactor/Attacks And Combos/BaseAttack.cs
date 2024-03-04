using DG.Tweening;
using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    protected PlayerManager _player;
    public DataAttack data;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerManager>();
    }

    public void Use()
    {
        if(!CharacterIsOnAllowedState())
        {
            return;
        }
        _player.animations.Animator.SetBool("isAttacking", true);
        _player.isAttacking = true;
        if(_player.lockCombat.isLockedOn)
        {
            _player.lockCombat.LookAtEnemySelected();
            _player.animations.PlayTargetAnimation(data.animation, true);
        }
        else
        {
            if(_player.lockCombat.selectedEnemy != null)
            {
                if (_player.lockCombat.distanceToAnimMove < Mathf.Abs(Vector3.Distance(_player.lockCombat.selectedEnemy.transform.position, _player.transform.position)) && _player.canMoveToEnemy)
                {
                    _player.animations.PlayTargetAnimation("roll", true);
                    _player.canMoveToEnemy = false;
                    _player.transform.DOLookAt(_player.lockCombat.selectedEnemy.transform.position, .2f);
                    _player.transform.DOMove(TargetOffset(_player.lockCombat.selectedEnemy.transform), .40f).OnComplete(() => _player.animations.PlayTargetAnimation(data.animation, true));
                }
                else
                {
                    _player.transform.DOLookAt(_player.lockCombat.selectedEnemy.transform.position, .2f);
                    _player.transform.DOMove(TargetOffset(_player.lockCombat.selectedEnemy.transform), .3f).OnComplete(() => _player.animations.PlayTargetAnimation(data.animation, true));
                }
            }                
            else
            {
                _player.animations.PlayTargetAnimation(data.animation, true);
            }
        }


        AddSoundEffects();
    }

    protected virtual void AddSoundEffects()
    {
        if(data.audio != null)
        {
            //PlayAudioClip
        }
    }
    public Vector3 TargetOffset(Transform target)
    {
        Vector3 position;
        position = target.position;
        return Vector3.MoveTowards(position, _player.transform.position, 1.5f);
    }
    private bool CharacterIsOnAllowedState()
    {
        return data.AllowedCharacterStates.Contains(_player.CurrentCharacterState);
    }
}
