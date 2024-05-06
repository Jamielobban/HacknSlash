using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;

public abstract class Interactive : MonoBehaviour
{
    [Header("INTERACTIVE AREA")]
    [SerializeField] protected List<Renderer> renderers;
    [SerializeField] protected List<Material> triggerMats;
    [SerializeField] protected List<Material> normalMats;
    [SerializeField] protected Image interactButtonImage;
    [SerializeField] protected Sprite psInteractSprite;
    [SerializeField] protected Sprite xboxInteractSprite;

    private bool canInteract = true;

    public Action OnInteract;
    public void SetCanInteract(bool _canInteract) { canInteract = _canInteract; UpdateSpriteAlpha(canInteract ? 1 : 0); } 
    public bool GetCanInteract => canInteract;
    protected void InteractPerformed() => OnInteract?.Invoke();
    void UpdateSpriteAlpha(float aValue) => interactButtonImage.DOColor(new Color(interactButtonImage.color.r, interactButtonImage.color.g, interactButtonImage.color.b, aValue), 0.5f);
    private void Awake()
    {
        for (int i = 0; i < triggerMats.Count(); i++)
            triggerMats[i] = new Material(triggerMats[i]);

        for (int i = 0; i < triggerMats.Count(); i++)
            normalMats[i] = new Material(normalMats[i]);

        var gamepad = Gamepad.current;
        interactButtonImage.sprite = gamepad is DualShockGamepad ? psInteractSprite : xboxInteractSprite;
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
