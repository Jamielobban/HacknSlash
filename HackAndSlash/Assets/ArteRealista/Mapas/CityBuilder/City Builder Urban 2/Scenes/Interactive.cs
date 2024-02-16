using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
    //InteractiveColor
    [SerializeField] Material triggerMat, normalMat;
    protected bool canInteract = true;

    public void ShowObjectInRange()
    {
        GetComponentInChildren<Renderer>().material = triggerMat;
    }

    public void HideObjectInRange()
    {
        GetComponentInChildren<Renderer>().material = normalMat;
    }
}
