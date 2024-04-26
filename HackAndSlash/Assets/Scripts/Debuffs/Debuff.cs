using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    public EnemyBase enemy;
    public bool isActive = false;
    public bool isStackeable = false;

    protected float duration;

    protected float _currentTimeDuration = 0f;

    protected virtual void Update()
    {
        if (isActive)
        {
            IsActiveUpdate();
        }
    }

    protected virtual void IsActiveUpdate()
    {
        _currentTimeDuration += Time.deltaTime;

        if (_currentTimeDuration >= duration)
        {
            _currentTimeDuration = 0;
            isActive = false;
        }
    }

    public virtual void ApplyDebuff(float time)
    {
        if(isActive && isStackeable)
        {
            duration += time;
        }
        else
        {
            duration = time;
            _currentTimeDuration = 0;
            isActive = true;
        }
    }
}