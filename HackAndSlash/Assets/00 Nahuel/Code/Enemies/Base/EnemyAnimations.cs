using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// -- Base Animations class, in Animator base animation as "Die", "Hit", "Stun" should be named equal always -- //
public class EnemyAnimations : MonoBehaviour
{
    protected Enemy _enemy;
    private Animator _anim;
    public Animator Animator => _anim;
    protected EnemyEvents _events;

    public List<SkinnedMeshRenderer> mats = new List<SkinnedMeshRenderer>();
    protected float _currentTime = 1.2f;

    protected virtual void Awake()
    {
        _events = transform.parent.GetComponent<EnemyEvents>();
        _anim = GetComponent<Animator>();
        _enemy = transform.parent.GetComponent<Enemy>();
        _events.OnIdle += () =>
        {
            StartCoroutine(DecreaseSpeedOverTime(0f, .5f));
        };
        _events.OnPatrolling += () => _anim.SetFloat("speed", 0.5f);
        _events.OnAttacking += () =>
        {
            StartCoroutine(DecreaseSpeedOverTime(0f, .5f));
        };
        _events.OnFollowing += () =>
        {
            StartCoroutine(IncreaseOverTime(0f, 1f));
        };
        
        _events.OnStun += () => PlayTargetAnimation("Stun", true);
        _events.OnDie += () => DeadAnimEnd();
        if(mats.Count > 0)
        {
            foreach (var mat in mats)
            {
                mat.material.SetFloat("_ShaderDisplacement", 1.2f);
            }
        }

    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        if(_enemy.isDead && mats.Count > 0)
        {
            _currentTime -= Time.deltaTime * 0.5f;
            foreach (var mat in mats)
            {
                mat.material.SetFloat("_ShaderDisplacement", _currentTime);
            }
            if(_currentTime <= 0f)
            {
                ManagerEnemies.Instance.AddEnemyScore(_enemy.stats.score);
                ManagerEnemies.Instance.AddEnemyKilled();
                EnemyDieApply();
            }
        }
    }

    public virtual void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        _anim.SetBool("isInteracting", isInteracting);
        _anim.CrossFade(targetAnimation, 0.2f);
    }
    protected virtual void DeadAnimEnd()
    {
        PlayTargetAnimation("Die", true);

        if(mats.Count <= 0)
        {
            this.Wait(Animator.GetCurrentAnimatorClipInfo(0).Length, () =>
            {
                ManagerEnemies.Instance.AddEnemyScore(_enemy.stats.score);
                ManagerEnemies.Instance.AddEnemyKilled();
                EnemyDieApply();
            });
        }
    }
    public void EnemyDieApply()
    {
        _currentTime = 1.2f;
        if (_enemy.isPooleable)
        {
            if(_enemy.spawner.GetComponent<SpawnerBase>())
            {
                _enemy.spawner?.GetComponent<SpawnerBase>()?.RemoveEnemy(_enemy);
            }
            else
            {
                ManagerEnemies.Instance.SetSpawnedEnemies(-1);
            }
            _enemy.spawner = null;
            _enemy.ResetEnemy();
            _enemy.gameObject.SetActive(false);
        }
        else
        {
            _enemy.gameObject.SetActive(false);
        }
    }

    public IEnumerator DecreaseSpeedOverTime(float targetValue, float _duration)
    {
        float currentSpeedAnimation = _anim.GetFloat(Constants.ANIM_VAR_SPEED);
        float elapsedTime = 0f;
        float duration = _duration;

        while (elapsedTime < duration)
        {
            float newAnimationSpeed = Mathf.Lerp(currentSpeedAnimation, targetValue, elapsedTime / duration);
            _anim.SetFloat(Constants.ANIM_VAR_SPEED, newAnimationSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _anim.SetFloat(Constants.ANIM_VAR_SPEED, 0f);
    }

    private IEnumerator IncreaseOverTime(float currentSpeed, float targetValue)
    {
        float elapsedTime = 0f;
        float duration = 0.3f;
        while (elapsedTime < duration)
        {
            float newSpeedAnimation = Mathf.Lerp(currentSpeed, targetValue, elapsedTime / duration);
            _anim.SetFloat(Constants.ANIM_VAR_SPEED, newSpeedAnimation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _anim.SetFloat(Constants.ANIM_VAR_SPEED, targetValue);
    }
}
