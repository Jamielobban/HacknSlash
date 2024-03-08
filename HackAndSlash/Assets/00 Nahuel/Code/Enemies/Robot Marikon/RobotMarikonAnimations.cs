using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMarikonAnimations : EnemyAnimations
{
    protected override void Start()
    {
        foreach (var mat in mats)
        {
            mat.material.SetFloat("_ShaderDisplacement", 1.2f);

        }
    }

    protected override void Update()
    {
        if(_enemy.isDead)
        {
            _currentTime -= Time.deltaTime / 2;
            foreach (var mat in mats)
            {
                mat.material.SetFloat("_ShaderDisplacement", _currentTime);
            }
            if(_currentTime <= -0.2f)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
