using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PyramidTeleport : Interactive, IInteractable
{
    [SerializeField] float period, verticalMoveTime, verticaDistance;   
    
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
        
    }

    public void Interact()
    {
        if (!canInteract) return;

        if (!firstTeleportDone)
        {
            firstTeleportDone = true;            
            //fade.DoTransition();
            loadingMenu.SetActive(true);
            Invoke(nameof(ActiveScene), 1f);
        }
    }

    private void ActiveScene() => GameManager.Instance.LoadLevel("03 Tutorial Combat", loadingFillBar);
}
