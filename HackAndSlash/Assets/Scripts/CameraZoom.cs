using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamZoom : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public float targetFOV = 60f;
    public float transitionDuration = 1f;

    private float initialFOV;
    private float timer;
    public bool isLerping;

    void Start()
    {
        if (freeLookCamera == null)
        {
            Debug.LogError("FreeLook Camera reference not set!");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (isLerping)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / transitionDuration);

            float currentFOV = Mathf.Lerp(initialFOV, targetFOV, t);
            freeLookCamera.m_Lens.FieldOfView = currentFOV;

            //if (t >= 1f)
            //{
            //    isLerping = false;
            //}
        }
    }

    public void StartFOVLerp(float duration)
    {
        transitionDuration = duration;
        initialFOV = freeLookCamera.m_Lens.FieldOfView;
        timer = 0f;
        isLerping = true;
    }
    public void SetFOVTarget(float target)
    {
        targetFOV = target;
    }
}