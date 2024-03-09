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
        _events.OnIdle += () => _anim.SetFloat("speed", 0);
        _events.OnPatrolling += () => _anim.SetFloat("speed", 0.5f);
        _events.OnAttacking += () => _anim.SetFloat("speed", 0);
        _events.OnFollowing += () => _anim.SetFloat("speed", 1);
        _events.OnStun += () => PlayTargetAnimation("Stun", true);
        _events.OnDie += () => DeadAnimEnd();
        //mats = new List<SkinnedMeshRenderer>(mats);
        if(mats.Count > 0)
        {
            foreach (var mat in mats)
            {
                mat.material.SetFloat("_ShaderDisplacement", 1f);
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
            if(_currentTime <= -0f)
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

        if (_enemy.isPooleable)
        {
            _enemy.spawner?.GetComponent<SpawnerBase>()?.RemoveEnemy(_enemy);

            ManagerEnemies.Instance.SetSpawnedEnemies(-1);
            _enemy.ResetEnemy();
            _enemy.gameObject.SetActive(false);
        }
        else
        {
            Destroy(_enemy.gameObject);
        }
    }
}
