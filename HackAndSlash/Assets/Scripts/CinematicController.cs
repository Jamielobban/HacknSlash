using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using Unity.VisualScripting;

public class CinematicController : MonoBehaviour
{
    [SerializeField] BlackCyborg bc;
    [SerializeField] Image blackImage;
    [SerializeField] VideoPlayer vp;

    [SerializeField] Image speachTextBackground, skipUiBackground;
    [SerializeField] TextMeshProUGUI speachText, skipUiText;

    [SerializeField] GameObject loadingMenu;
    [SerializeField] Image loadingFillBar;

    [SerializeField] GameObject screen;

    bool firstTime  = true;
    void CanSkip()
    {
        menuInputs.Enable();
        Color c = skipUiBackground.color;
        c.a = 0.9f;
        Color c2 = skipUiText.color;
        c2.a = 1;
        skipUiBackground.DOColor(c, 0.5f);
        skipUiText.DOColor(c2, 0.5f);
    }
    void CanNotSkip()
    {
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

    MenuInputs menuInputs;
    FadeScript fadeScript;
    bool canSkipIntro = true;
    // Start is called before the first frame update
    void OffScreen() => screen.SetActive(false);  
    void ChangeSceneFade() { fadeScript.FadeOut(1); Invoke(nameof(OffScreen), 1.2f); Invoke(nameof(ChangeScene),2.5f); }
    void StartSpeach() { bc.Speak(); }
    void StartVideo() { vp.Play(); }
    private void Awake()
    {
        vp.Play();
    }
    void Start()
    {
        menuInputs = new MenuInputs();
        menuInputs.UiInputs.SkipIntro.performed += SkipIntro;
        loadingMenu.SetActive(false);

        vp.Pause();
        fadeScript = new FadeScript(blackImage);
        fadeScript.FadeIn(1);
        Invoke(nameof(StartSpeach), 0.5f);
        Invoke(nameof(StartVideo), 0.5f);
        Invoke(nameof(CanSkip), 1);
        Invoke(nameof(CanNotSkip), 17.5f);
        Invoke(nameof(ChangeSceneFade), 20);
    }

    private void OnDestroy() { menuInputs.Disable(); }

    void ChangeScene()
    {
        loadingMenu.SetActive(true);
        Invoke(nameof(ActiveScene), 1f);
    }

    private void ActiveScene() => GameManager.Instance.LoadLevel(Constants.SCENE_TUTORIAL_COMBOS, loadingFillBar);

    void SkipIntro(InputAction.CallbackContext context)
    {
        if (!canSkipIntro) return;

        CanNotSkip();

        Color c = speachTextBackground.color;
        Color c2 = speachText.color;
        c.a = 0;
        c2.a = 0;
        speachTextBackground.DOColor(c, 1);
        speachText.DOColor(c2, 1);        
        //AudioManager.Instance.FadeEffect(0, 1); 
        DOVirtual.Float(1, 0, 0.9f, (v) => { vp.SetDirectAudioVolume(0, v); });
        ChangeSceneFade();
    }

}
