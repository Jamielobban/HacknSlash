using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCameraShake : MonoBehaviour
{
    ObjectShake cameraShake;
    // Start is called before the first frame update
    void Start()
    {
        cameraShake = FindAnyObjectByType<ObjectShake>();
    }
    public void SetShakeDuration(float duration)
    {
        cameraShake.TriggerShakeDuration(duration);
    }
    public void SetShakeMagnitude(float magnitude)
    {
        cameraShake.TriggerShakeMagnitude(magnitude);

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
