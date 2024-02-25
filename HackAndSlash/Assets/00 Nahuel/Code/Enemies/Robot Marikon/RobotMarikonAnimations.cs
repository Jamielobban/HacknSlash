using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMarikonAnimations : EnemyAnimations
{
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
