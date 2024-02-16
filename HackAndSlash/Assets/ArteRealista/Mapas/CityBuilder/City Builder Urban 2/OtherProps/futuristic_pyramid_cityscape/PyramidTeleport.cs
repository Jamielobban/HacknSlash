using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PyramidTeleport : Interactive, IInteractable
{
    [SerializeField] float period, verticalMoveTime, verticaDistance;

    private void Start()
    {
        Vector3 startPosition = this.transform.position;
        transform.DORotate(new Vector3(0, 360, 0) + this.transform.rotation.eulerAngles, period, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        transform.DOMoveY(startPosition.y + verticaDistance, verticalMoveTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public void Interact()
    {
        if (!canInteract) return;
    }
}
