using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutoPyramidManager : MonoBehaviour
{
    [SerializeField] AudioListener listener;
    [SerializeField] PyramidTeleport pyramid;
    [SerializeField] GameObject loadingMenu;
    [SerializeField] Image loadingFillBar;
    [SerializeField] PlayerControl playerControl;
    [SerializeField] Image fadeImage;
    FadeScript fade;

    void PlayerActive() => playerControl.enabled = true;
    void StartFade() => fade.FadeIn(1.8f);

    // Start is called before the first frame update
    void Awake()
    {
        fade = new FadeScript(fadeImage);
        playerControl.enabled = false;
    }

    private void Start()
    {
        fade.FadeIn(1f);
        pyramid.OnInteract += ChangeScene;
        playerControl.GetComponentInChildren<Animator>().CrossFadeInFixedTime("Idle", 0.2f);
        Invoke(nameof(PlayerActive), 2.1f);
    }    

    void ChangeScene()
    {
       /// listener.enabled = false;
       // playerControl.enabled = false;
        pyramid.OnInteract -= ChangeScene;
        loadingMenu.SetActive(true);
        Invoke(nameof(ActiveScene), 1f);
    }
    private void ActiveScene() => GameManager.Instance.LoadLevel(Constants.SCENE_TUTORIALCOMBAT, loadingFillBar);
}
