using UnityEngine;

public class ObjectShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = originalPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = originalPosition;
        }
    }

    public void TriggerShakeDuration(float duration)
    {
        shakeDuration = duration;
    }
    public void TriggerShakeMagnitude(float magnitude)
    {
        shakeMagnitude = magnitude;
    }
}
