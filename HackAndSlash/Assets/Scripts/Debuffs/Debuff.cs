using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    public bool isActive = false;
    public bool isStackeable = false;

    public float duration;
    protected float _currentDuration;

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

        if (_currentTimeDuration >= _currentDuration)
        {
            _currentTimeDuration = 0;
            isActive = false;
        }
    }

    public virtual void ApplyDebuff(float damage)
    {
        if(isActive && isStackeable)
        {
            _currentDuration += duration;
        }
        else
        {
            _currentDuration = duration;
            _currentTimeDuration = 0;
            isActive = true;
        }
    }
}