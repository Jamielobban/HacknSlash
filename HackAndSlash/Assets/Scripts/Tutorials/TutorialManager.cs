using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class TutorialManager : MonoBehaviour
{
    FadeScript fade;
    BlackCyborg blackCyborg;
    TutorialComboManager tutorialCM;
    [SerializeField] PyramidTeleport pyramid;

    [Header("SceneElements")]
    [SerializeField] AudioListener audioListener;
    [SerializeField] GameObject blackCyborgObjectiveMarker;
    [SerializeField] GameObject pyramidObjectiveMarker;
    [SerializeField] GameObject loadingMenu;
    [SerializeField] Image loadingFillBar;
    [SerializeField] Image fadeImage;

    [SerializeField] Animator animatorLeft;
    [SerializeField] Animator animatorRight;

    [SerializeField] BoxCollider colliderBlockingPath;

    Enums.NewTutorialState tutorialState;
    public FadeScript GetFade => fade;
    public void EndTutorial() => Invoke(nameof(ChangeScene), 2.8f);
    public void MuteInSeconds(float inSeconds) => Invoke(nameof(Mute), inSeconds);
    void Mute() => audioListener.enabled = false;
    private void Awake()
    {
        GameManager.Instance.UpdateState(Enums.GameState.Tutorial);
        fade = new FadeScript(fadeImage);
        tutorialCM = GetComponent<TutorialComboManager>();
        blackCyborg = FindObjectOfType<BlackCyborg>();
        blackCyborg.OnInteract += RobotInteraction;
        tutorialCM.OnCombosListComplete += PhaseComplete;
        tutorialCM.OnTutorialComboComplete += ()=> { tutorialState = Enums.NewTutorialState.FINISHED; };
        blackCyborg.OnConversationEnded += MoveRobots;
    }
    void Start()
    {
        pyramid.OnInteract += ChangeScene;
        animatorLeft.SetBool("idle", true);
        animatorRight.SetBool("idle", true);
        fade.FadeIn(1.8f);
        tutorialState = Enums.NewTutorialState.INACTIVE;
        blackCyborgObjectiveMarker.SetActive(true);
        blackCyborg.SetCanInteract(true);
        loadingMenu.SetActive(false);
    }
    private void PhaseComplete()
    {
        blackCyborgObjectiveMarker.SetActive(true);
        blackCyborg.SetCanInteract(true);
    }
    private void RobotInteraction()
    {
        blackCyborgObjectiveMarker.SetActive(false);
        blackCyborg.Speak();
        switch (tutorialState)
        {
            case Enums.NewTutorialState.INACTIVE:
                tutorialState = Enums.NewTutorialState.COMBOS;
                tutorialCM.StartCurrentCombosList();
                break;
            case Enums.NewTutorialState.COMBOS:
                tutorialCM.StartCurrentCombosList();
                break;    
            default:
                break;
        }
    }
    void ChangeScene()
    {
        loadingMenu.SetActive(true);
        Invoke(nameof(ActiveScene), 1f);
    }
    private void ActiveScene() => GameManager.Instance.LoadLevel(Constants.SCENE_TUTORIALCOMBAT, loadingFillBar);
    private void OnDestroy()
    {
        blackCyborg.OnInteract -= RobotInteraction;       
        tutorialCM.OnCombosListComplete -= PhaseComplete;
    }
    public void MoveRobots()
    {
        animatorRight.transform.DOMoveX(8.27f, 0.8f);
        animatorLeft.transform.DOMoveX(-0.26f, 0.8f);
        animatorLeft.CrossFadeInFixedTime("WalkLeftCombat", 0.1f);
        animatorRight.CrossFadeInFixedTime("WalkRightCombat", 0.1f);
        Invoke(nameof(StopRobots), 0.7f);
    }
    void StopRobots()
    {
        colliderBlockingPath.enabled = false;        
        animatorLeft.CrossFadeInFixedTime("IdleArmed", 0.1f);
        animatorRight.CrossFadeInFixedTime("IdleArmed", 0.1f);
    }    

}
