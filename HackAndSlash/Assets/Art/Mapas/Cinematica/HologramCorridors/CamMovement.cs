using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class CamMovement : MonoBehaviour
{
    
    [SerializeField] MainMenuManager mainMenuManager;
    Sequence sequence;    
    int doorsOpened = 0;
    void Start()
    {       
        Tween moveTween = this.transform.DOMoveX(15.46f, 20).SetEase(Ease.InOutSine); //3.79f //1.878
        this.transform.DORotate(new Vector3(2.878f, 90, 0), 15.46f).SetEase(Ease.InOutSine);

        sequence = DOTween.Sequence();
        sequence.Append(moveTween);
    }
    private void Update()
    {
        Debug.Log(doorsOpened);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Animator>(out Animator anim))
        {
            if (this.transform.position.x > 6)             
                mainMenuManager.MainMenuEnter();
                            
            anim.CrossFadeInFixedTime("DoorOpen", 0.2f);
            if(doorsOpened < 8)
            {
                AudioManager.Instance.PlayFx(Enums.Effects.DoorOpen); 

            }
            doorsOpened++; 
            
           
        }
    }

}
