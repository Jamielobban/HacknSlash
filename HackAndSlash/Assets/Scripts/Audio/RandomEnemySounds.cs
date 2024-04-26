using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RandomEnemySounds : MonoBehaviour
{
    private float _currentTime = 0f;
    public float timeToSound;
    public MMFeedbacks randomSFX;
    void Update()
    {

        _currentTime += Time.deltaTime;
        if (_currentTime >= timeToSound) 
        {
            _currentTime = 0f;
            randomSFX.PlayFeedbacks();
        }


    }
}
