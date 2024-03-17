using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    FadeScript fade;
    BlackCyborg blackCyborg;
    TutorialComboManager tutorialCM;
    PyramidTeleport pyramid;

    [Header("SceneElements")]
    [SerializeField] GameObject blackCyborgObjectiveMarker;
    [SerializeField] GameObject enemyHealthUI;
    [SerializeField] BoxCollider enemyDamageableCol;
    [SerializeField] GameObject pyramidObjectiveMarker;
    [SerializeField] GameObject loadingMenu;
    [SerializeField] Image loadingFillBar;
    [SerializeField] Image fadeImage;

    Enums.NewTutorialState tutorialState;

    private void Awake()
    {
        GameManager.Instance.UpdateState(Enums.GameState.Tutorial);
        fade = new FadeScript(fadeImage);
        tutorialCM = GetComponent<TutorialComboManager>();
        blackCyborg = FindObjectOfType<BlackCyborg>();
        pyramid = FindObjectOfType<PyramidTeleport>();
        blackCyborg.OnInteract += RobotInteraction;
        pyramid.OnInteract += PyramidInteraction;
        tutorialCM.OnCombosListComplete += PhaseComplete;
        tutorialCM.OnTutorialComboComplete += ChangeStateToPyramid;
    }
    void Start()
    {
        //AudioManager.Instance.PlayMusic(Enums.Music.MainTheme);
        fade.FadeIn(1.8f);
        tutorialState = Enums.NewTutorialState.INACTIVE;
        blackCyborgObjectiveMarker.SetActive(true);
        enemyHealthUI.SetActive(false);
        enemyDamageableCol.enabled = false;
        pyramidObjectiveMarker.SetActive(false);
        blackCyborg.SetCanInteract(true);
        pyramid.SetCanInteract(false);
        loadingMenu.SetActive(false);
    }

    private void PhaseComplete()
    {
        blackCyborgObjectiveMarker.SetActive(true);
        blackCyborg.NextDialogue();
        blackCyborg.SetCanInteract(true);
    }

    private void ChangeStateToPyramid() => tutorialState = Enums.NewTutorialState.PYRAMIDE;   

    private void RobotInteraction()
    {
        blackCyborgObjectiveMarker.SetActive(false);
        blackCyborg.Speak();
        switch (tutorialState)
        {
            case Enums.NewTutorialState.INACTIVE:
                tutorialState = Enums.NewTutorialState.COMBOS;
                blackCyborg.SetCanInteract(false);
                tutorialCM.StartCurrentCombosList();
                break;
            case Enums.NewTutorialState.COMBOS:
                tutorialCM.StartCurrentCombosList();
                blackCyborg.SetCanInteract(false);
                break;
            case Enums.NewTutorialState.PYRAMIDE:
                pyramid.SetCanInteract(true);
                pyramidObjectiveMarker.SetActive(true);
                enemyHealthUI.SetActive(true);
                enemyDamageableCol.enabled = true;
                break;
            default:
                break;
        }
    }

    private void PyramidInteraction()
    {
        pyramid.SetCanInteract(false);
        tutorialState = Enums.NewTutorialState.FINISHED;
        loadingMenu.SetActive(true);
        Invoke(nameof(ActiveScene), 1f);    
    }

    private void ActiveScene() => GameManager.Instance.LoadLevel(Constants.SCENE_TUTORIALCOMBAT, loadingFillBar);

    private void OnDestroy()
    {
        blackCyborg.OnInteract -= RobotInteraction;
        pyramid.OnInteract -= PyramidInteraction;
        tutorialCM.OnCombosListComplete -= PhaseComplete;
        tutorialCM.OnTutorialComboComplete -= ChangeStateToPyramid;
    }
}
