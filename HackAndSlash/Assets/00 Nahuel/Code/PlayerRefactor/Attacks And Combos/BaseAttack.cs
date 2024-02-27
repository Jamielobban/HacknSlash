using UnityEngine;

public class BaseAttack : MonoBehaviour
{
    protected PlayerManager _player;
    public DataAttack data;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerManager>();
    }

    public GameObject currentNearEnemy;
    private float _nearDistance;

    protected virtual GameObject CheckNearEnemy()
    {
        if(_player.collision.TouchingEnemies.Count > 0)
        {
            _nearDistance = 50.0f;
            foreach (var enemy in _player.collision.TouchingEnemies)
            {
                if(Vector3.Distance(_player.gameObject.transform.position, enemy.transform.position) < _nearDistance)
                {
                    _nearDistance = Vector3.Distance(_player.gameObject.transform.position, enemy.transform.position);
                    currentNearEnemy = enemy;
                }
            }
            return currentNearEnemy;
        }
        return null;
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
