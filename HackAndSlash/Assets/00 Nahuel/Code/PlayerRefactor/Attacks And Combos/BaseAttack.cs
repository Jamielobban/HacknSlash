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

        _player.animations.PlayTargetAnimation(data.animation, true);
        AddSoundEffects();
    }

    protected virtual void AddSoundEffects()
    {
        if(data.audio != null)
        {
            //PlayAudioClip
        }
    }

    private bool CharacterIsOnAllowedState()
    {
        return data.AllowedCharacterStates.Contains(_player.CurrentCharacterState);
    }
}
