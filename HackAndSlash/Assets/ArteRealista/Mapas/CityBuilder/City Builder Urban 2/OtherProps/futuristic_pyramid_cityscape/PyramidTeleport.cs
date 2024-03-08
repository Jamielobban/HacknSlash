using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PyramidTeleport : Interactive, IInteractable
{
    [SerializeField] float period, verticalMoveTime, verticaDistance;
    [SerializeField] BlackCyborgSoldier soldier;
    [SerializeField] GameObject objectiveMarker;
    [SerializeField] FadeScript fade;
    bool firstTeleportDone = false;

    public GameObject loadingMenu;
    public Image loadingFillBar;

    private void Start()
    {
        Vector3 startPosition = this.transform.position;
        transform.DORotate(new Vector3(0, 360, 0) + this.transform.rotation.eulerAngles, period, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        transform.DOMoveY(startPosition.y + verticaDistance, verticalMoveTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        loadingMenu.SetActive(false);
    }

    private void Update()
    {
        if (!objectiveMarker.activeSelf && !firstTeleportDone && soldier.TutorialState == Enums.TutorialState.FINISHED)
        {
            if(!soldier.objectiveMarker.activeSelf)
                objectiveMarker.SetActive(true);
        }
    }

    public void Interact()
    {
        if (!canInteract) return;

        if (!firstTeleportDone)
        {
            firstTeleportDone = true;
            objectiveMarker.SetActive(false);
            //fade.DoTransition();
            loadingMenu.SetActive(true);
            Invoke(nameof(ActiveScene), .15f);
        }
    }

    private void ActiveScene() => GameManager.Instance.LoadLevel("DanielIceMap", loadingFillBar);
}
