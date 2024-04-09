using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    BlackCyborg blackCyborg;
    NaveMov nave;
    FadeScript fade;

    [Header("SceneElements")]
    [SerializeField] Image blackImage;
    [SerializeField] GameObject canvasLoadingBar;
    [SerializeField] Image imageFillLoading;

    void KeepTalking() => StartCoroutine(BlackCyborgMonologue());
    void ActivateLoadingBar() => canvasLoadingBar.SetActive(true);
    void EndScene()
    {
        fade.FadeOut(0.5f);
        Invoke(nameof(ActivateLoadingBar), 0.8f);
        //AudioManager.Instance.FadeMusic(1.5f, 0f);
        Invoke(nameof(ActivateScene), 1f);
    }

    void Awake()
    {
        fade = new FadeScript(blackImage);
        blackCyborg = FindObjectOfType<BlackCyborg>();
        nave = FindObjectOfType<NaveMov>();
        blackCyborg.OnDialogueLineDone += KeepTalking;
        blackCyborg.OnDialogueEnded += EndScene;
    }

    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.Instance.PlayMusic(Enums.Music.Helicopter);
        GameManager.Instance.UpdateState(Enums.GameState.Tutorial);
        canvasLoadingBar.SetActive(false);
        blackImage.gameObject.SetActive(true);
        fade.FadeIn(1.5f);
        nave.StartMove();
        StartCoroutine(BlackCyborgMonologue(1.5f));
    }

    IEnumerator BlackCyborgMonologue(float enterWaitTime = 1)
    {
        yield return new WaitForSeconds(enterWaitTime);
        blackCyborg.Speak();
        blackCyborg.NextDialogue();
    }

    void ActivateScene() => GameManager.Instance.LoadLevel(Constants.SCENE_TUTORIAL, imageFillLoading);    

    private void OnDestroy()
    {
        blackCyborg.OnDialogueLineDone -= KeepTalking;
        blackCyborg.OnDialogueEnded -= EndScene;
    }
}
