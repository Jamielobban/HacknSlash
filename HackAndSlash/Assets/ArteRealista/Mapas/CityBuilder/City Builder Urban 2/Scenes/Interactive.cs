using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
    [Header("INTERACTIVE AREA")]
    [SerializeField] Material triggerMat, normalMat;
    protected bool canInteract = true;

    public virtual void ShowObjectInRange()
    {
        GetComponentInChildren<Renderer>().material = triggerMat;
    }

    public virtual void HideObjectInRange()
    {
        GetComponentInChildren<Renderer>().material = normalMat;
    }
}
