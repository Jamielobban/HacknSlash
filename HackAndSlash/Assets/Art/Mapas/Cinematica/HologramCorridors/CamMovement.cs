using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using UnityEngine.InputSystem;

public class CamMovement : MonoBehaviour
{
    
    [SerializeField] MainMenuManager mainMenuManager;
    [SerializeField] Image blackImage;
    [SerializeField] Image speachTextBackground, skipUiBackground;
    [SerializeField] TextMeshProUGUI speachText, skipUiText;


    MenuInputs menuInputs;
    FadeScript fadeScript;
    bool canSkipIntro = true;

    Sequence sequence;    
    int doorsOpened = 0;

    void CanSkip() { 
        menuInputs.Enable();
        Color c = skipUiBackground.color;
        c.a = 0.835f;
        Color c2 = skipUiText.color;
        c2.a = 1;
        skipUiBackground.DOColor(c, 0.5f);
        skipUiText.DOColor(c2, 0.5f);
    }
    void CanNotSkip() { 
        canSkipIntro = false; 
        menuInputs.UiInputs.SkipIntro.performed -= SkipIntro; 
        Color c = skipUiBackground.color;
        c.a = 0;
        Color c2 = skipUiText.color;    
        c2.a = 0;
        skipUiBackground.DOColor(c, 1);
        skipUiText.DOColor(c2, 1); 
        menuInputs.Disable(); 
    }
    void Start()
    {       
        fadeScript = new FadeScript(blackImage);
        menuInputs = new MenuInputs();
        menuInputs.UiInputs.SkipIntro.performed += SkipIntro;
        
        Invoke(nameof(CanSkip), 1);
        Invoke(nameof(CanNotSkip), 10);
        Tween moveTween = this.transform.DOMoveX(15.46f, 20).SetEase(Ease.InOutSine); //3.79f //1.878
        Tween rotTween = this.transform.DORotate(new Vector3(2.878f, 90, 0), 15.46f).SetEase(Ease.InOutSine);

        sequence = DOTween.Sequence();
        sequence.Join(moveTween).Join(rotTween);
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

    void SkipIntro(InputAction.CallbackContext context)
    {
        if (!canSkipIntro) return;

        CanNotSkip();
        StartCoroutine(SkipIntroCoroutine());
    }

    IEnumerator SkipIntroCoroutine()
    {
        Color c = speachTextBackground.color;
        Color c2 = speachText.color;
        c.a = 0;
        c2.a = 0;
        speachTextBackground.DOColor(c, 1);
        speachText.DOColor(c2, 1);
        fadeScript.FadeOut(1);
        AudioManager.Instance.FadeEffect(0, 1);
        yield return new WaitForSeconds(1.1f);
        AudioManager.Instance.audioFx.Stop();
        sequence.Complete();
        yield return new WaitForSeconds(.1f);
        fadeScript.FadeIn(1);
        AudioManager.Instance.FadeEffect(1, 1);
    }

    private void OnDestroy() => menuInputs.Disable();
}
