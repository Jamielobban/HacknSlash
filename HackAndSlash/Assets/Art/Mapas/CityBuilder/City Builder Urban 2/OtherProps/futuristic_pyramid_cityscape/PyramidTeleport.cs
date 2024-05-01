using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class PyramidTeleport : Interactive, IInteractable
{
    [SerializeField] float period, verticalMoveTime, verticaDistance;
    [SerializeField] Transform marker;
    [SerializeField] BlackCyborg bc;

    private void Start()
    {
        SetCanInteract(false);
        Vector3 startPosition = this.transform.position;
        Vector3 startCrossPosition = interactCross.transform.position;
        Vector3 startMarkerPosition = marker.position;
        transform.DORotate(new Vector3(0, 360, 0) + this.transform.rotation.eulerAngles, period, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        interactCross.transform.DOMoveY(startCrossPosition.y + verticaDistance, verticalMoveTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        marker.DOMoveY(startMarkerPosition.y + verticaDistance, verticalMoveTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        transform.DOMoveY(startPosition.y + verticaDistance, verticalMoveTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        marker.parent.gameObject.SetActive(false);
        bc.OnConversationEnded += PyramidStep;
    }
    public void Interact()
    {
        if (!GetCanInteract) return;
        SetCanInteract(false);
        InteractPerformed();      
    }

    void PyramidStep()
    {
        SetCanInteract(true);
        marker.parent.gameObject.SetActive(true);
    }
}
