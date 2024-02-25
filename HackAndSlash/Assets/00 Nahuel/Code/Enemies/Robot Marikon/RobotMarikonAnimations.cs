using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMarikonAnimations : EnemyAnimations
{
    public List<Material> mats = new List<Material>();

    private float _currentTime = 1.2f;

    protected override void Start()
    {
        foreach (var mat in mats)
        {
            mat.SetFloat("_ShaderDisplacement", 1.2f);
            //mat.SetFloat("_ShaderHologramDisplacement", 1 - _currentTime);
            //mat.SetFloat("_ShaderDissolve", 1 - _currentTime);

        }
    }

    protected override void Update()
    {
        base.Update();
        if(_enemy.isDead)
        {
            _currentTime -= Time.deltaTime / 2;
            foreach (var mat in mats)
            {
                mat.SetFloat("_ShaderDisplacement", _currentTime);
            }
            if(_currentTime <= -0.2f)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
    protected override void DeadAnimEnd()
    {
        PlayTargetAnimation("Die", true);
        this.Wait(Animator.GetCurrentAnimatorClipInfo(0).Length, () =>
        {
            //Respawn?
            if (_enemy.isPooleable)
            {
                RoomManager.Instance.RemoveEnemy(_enemy.gameObject);
            }
            else
            {
                //Activas Shader
            }
            //_enemy.gameObject.SetActive(false);
        });
    }
}
