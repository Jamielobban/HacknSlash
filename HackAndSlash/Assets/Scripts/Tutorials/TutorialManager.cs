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

    [SerializeField] Transform player;

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

    [SerializeField] Image joytstickLeft;
    [SerializeField] Image joytsticRight;

    Sequence sequenceLeft;
    Sequence sequenceRight;

    PlayerControl pCtrl;
    ControllerManager cMgr;

    bool playerMoved = false, camRotated = false;
    Vector3 playerInitPos, camInitRot;

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
        AudioManager.Instance.PlayMusic(Enums.Music.MusicaTutoNou);
        pyramid.OnInteract += ChangeScene;
        animatorLeft.SetBool("idle", true);
        animatorRight.SetBool("idle", true);
        fade.FadeIn(1.8f);
        tutorialState = Enums.NewTutorialState.INACTIVE;
        blackCyborgObjectiveMarker.SetActive(true);
        blackCyborg.SetCanInteract(true);
        loadingMenu.SetActive(false);
        playerInitPos = player.position;
        camInitRot = Camera.main.transform.eulerAngles;
        sequenceLeft = DOTween.Sequence();
        sequenceRight = DOTween.Sequence();
        pCtrl = player.GetComponent<PlayerControl>();
        cMgr = pCtrl.controller;

        sequenceLeft.Append(DOVirtual.Float(0, 1, 0.5f, (alpha) => { joytstickLeft.color = new Color(joytstickLeft.color.r, joytstickLeft.color.g, joytstickLeft.color.b, alpha); }))
            .Append(DOVirtual.Float(1, 0, 1, (alpha) => { joytstickLeft.color = new Color(joytstickLeft.color.r, joytstickLeft.color.g, joytstickLeft.color.b, alpha); })).SetLoops(-1).Play();
        

        sequenceRight.Append(DOVirtual.Float(0, 1, 0.5f, (alpha) => { joytsticRight.color = new Color(joytsticRight.color.r, joytsticRight.color.g, joytsticRight.color.b, alpha); }))
            .Append(DOVirtual.Float(1, 0, 1, (alpha) => { joytsticRight.color = new Color(joytsticRight.color.r, joytsticRight.color.g, joytsticRight.color.b, alpha); })).SetLoops(-1).Play(); 
        
    }

    private void Update()
    {
        if (!playerMoved && Vector3.Distance(playerInitPos, player.position) > 0.5f)
        {
            playerMoved = true;
            sequenceLeft.Kill();
            DOVirtual.Float(joytstickLeft.color.a, 1, 0.35f, (alpha) => { joytstickLeft.color = new Color(joytstickLeft.color.r, joytstickLeft.color.g, joytstickLeft.color.b, alpha); }).OnComplete(() =>
            { DOVirtual.Float(1, 0, 0.7f, (scale) => { joytstickLeft.transform.localScale = Vector3.one * scale; }); });
        }

        if (!camRotated && cMgr.RightStickValue().magnitude != 0)
        {
            camRotated = true;
            sequenceRight.Kill();
            DOVirtual.Float(joytsticRight.color.a, 1, 0.35f, (alpha) => { joytsticRight.color = new Color(joytsticRight.color.r, joytsticRight.color.g, joytsticRight.color.b, alpha); }).OnComplete(() =>
            { DOVirtual.Float(1, 0, 0.7f, (scale) => { joytsticRight.transform.localScale = Vector3.one * scale; }); });
        }
    }

    private void PhaseComplete()
    {
        RobotInteraction();
        //blackCyborgObjectiveMarker.SetActive(true);
        //blackCyborg.SetCanInteract(true);
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
        AudioManager.Instance.PlayFx(Enums.Effects.FootstepsRobot);
        AudioManager.Instance.PlayFx(Enums.Effects.FootstepsRobot);
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
