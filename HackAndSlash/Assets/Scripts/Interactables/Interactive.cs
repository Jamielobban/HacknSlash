using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public abstract class Interactive : MonoBehaviour
{
    [Header("INTERACTIVE AREA")]
    [SerializeField] protected List<Renderer> renderers;
    [SerializeField] protected List<Material> triggerMats;
    [SerializeField] protected List<Material> normalMats;
    [SerializeField] protected Image interactCross;

    private bool canInteract = true;

    public Action OnInteract;
    public void SetCanInteract(bool _canInteract) { canInteract = _canInteract; UpdateSpriteAlpha(canInteract ? 1 : 0); } 
    public bool GetCanInteract => canInteract;
    protected void InteractPerformed() => OnInteract?.Invoke();
    void UpdateSpriteAlpha(float aValue) => interactCross.DOColor(new Color(interactCross.color.r, interactCross.color.g, interactCross.color.b, aValue), 0.5f);
    private void Awake()
    {
        for (int i = 0; i < triggerMats.Count(); i++)
            triggerMats[i] = new Material(triggerMats[i]);

        for (int i = 0; i < triggerMats.Count(); i++)
            normalMats[i] = new Material(normalMats[i]);
    }

    private void OnEnable() => UpdateSpriteAlpha(canInteract ? 1 : 0);    

    private void FixedUpdate()
    {
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
