using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public abstract class Interactive : MonoBehaviour
{
    [Header("INTERACTIVE AREA")]
    [SerializeField] protected List<Renderer> renderers;
    [SerializeField] protected List<Material> triggerMats;
    [SerializeField] protected List<Material> normalMats;
    protected bool canInteract = true;
    public void SetCanInteract(bool _canInteract) => canInteract = _canInteract;

    public Action OnInteract;
    protected void InteractPerformed() => OnInteract?.Invoke();

    private void Awake()
    {
        Debug.Log("fent");

        for (int i = 0; i < triggerMats.Count(); i++)
            triggerMats[i] = new Material(triggerMats[i]);

        for (int i = 0; i < triggerMats.Count(); i++)
            normalMats[i] = new Material(normalMats[i]);
    }

    private void FixedUpdate()
    {
        //Debug.Log("fent");

        if (!canInteract && renderers.First().material != normalMats.First())
            for (int i = 0; i < renderers.Count(); i++)
                renderers[i].material = normalMats[i];

    }

    public virtual void ShowObjectInRange()
    {
        if(!canInteract) return;

        for (int i = 0; i < renderers.Count(); i++)
            renderers[i].material = triggerMats[i];
    }

    public virtual void HideObjectInRange()
    {
        for (int i = 0; i < renderers.Count(); i++)
            renderers[i].material = normalMats[i];
    }

}
