using FMODUnity;
using MoreMountains.Feedbacks;
using UnityEngine;

public class RandomEnemySounds : MonoBehaviour
{
    private float _currentTime = 0f;
    public float timeToSound;
    public MMFeedbacks randomSFX;
    public EventReference randomSound;
    void Update()
    {

        _currentTime += Time.deltaTime;
        if (_currentTime >= timeToSound) 
        {
            _currentTime = 0f;
            randomSFX.PlayFeedbacks();
            AudioManager.Instance.PlayFx(randomSound, transform.position);
        }


    }
}
