using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] SpinPyramid spinPyramid;
    [SerializeField] List<TextMeshProUGUI> pyramidMenuTexts;
    [SerializeField] TextMeshProUGUI textTvUp;
    [SerializeField] TextMeshProUGUI textTvDown;
    [SerializeField] RectTransform bigTvCanvas;
    [SerializeField] TextMeshProUGUI[] bigTvSliderTexts;
    [SerializeField] Color32 customGrey;
    [SerializeField] Color32 customRed;

    Button tvUpButton;
    Button tvDownButton;

    MenuInputs menuInputs;
    Dictionary<string, Action> menuActions = new Dictionary<string, Action>();
    int lastIndex = -1;
    bool started = false;

    Transform cam;

    public GameObject firstSelectedObejct;

    public GameObject loadingGo;
    public Image fillLoadingGo;

    public void MainMenuEnter() => Invoke(nameof(StartShowMenu), 0.5f);
    public void RotatitionDone() => EventSystem.current.SetSelectedGameObject(firstSelectedObejct);
    void StartShowMenu() { pyramidMenuTexts.ForEach(text => { Color c = text.color; text.DOColor(text.text == "GAME" ? Color.yellow : new Color(c.r, c.g, c.b, 1), 1.5f).SetEase(Ease.InExpo).OnComplete(() => { started = true; if (pyramidMenuTexts.IndexOf(text) == pyramidMenuTexts.Count - 1) spinPyramid.StartListening(); }); }); }
    void StartGame()  
    {
        loadingGo.SetActive(true);
        GameManager.Instance.UpdateState(Enums.GameState.Playing);
        GameManager.Instance.isTutorialCompleted = true;
        GameManager.Instance.LoadLevel(Constants.SCENE_MAIN, fillLoadingGo);
    }
    void StartTutorial()
    {
        loadingGo.SetActive(true);
        GameManager.Instance.UpdateState(Enums.GameState.Tutorial);
        GameManager.Instance.LoadLevel(Constants.SCENE_TUTORIAL, fillLoadingGo);
    }

    void Exitgame() => Application.Quit();
    void ShowTeam() { bigTvCanvas.GetChild(0).gameObject.SetActive(true); bigTvCanvas.GetChild(1).gameObject.SetActive(false); Debug.Log("ShowTeam"); }
    void ShowEnti() { bigTvCanvas.GetChild(0).gameObject.SetActive(false); bigTvCanvas.GetChild(1).gameObject.SetActive(true); Debug.Log("ShowEnti"); }
    void ShowSfx() { bigTvCanvas.GetChild(2).gameObject.SetActive(true); bigTvCanvas.GetChild(3).gameObject.SetActive(false); Debug.Log("Sfx"); }
    void ShowMusic() { bigTvCanvas.GetChild(2).gameObject.SetActive(false); bigTvCanvas.GetChild(3).gameObject.SetActive(true); Debug.Log("Music"); }
    void ResetTvs()
    {
        tvUpButton.onClick.RemoveAllListeners(); tvDownButton.onClick.RemoveAllListeners(); textTvUp.text = ""; textTvDown.text = "";
        bigTvCanvas.GetComponentsInChildren<RectTransform>(false).Where(t => t != bigTvCanvas && t.parent == bigTvCanvas).ToList().ForEach(t => t.gameObject.SetActive(false));
    }

    void Start()
    {
        cam = Camera.main.transform;
        tvUpButton = textTvUp.GetComponentInParent<Button>();
        tvDownButton = textTvDown.GetComponentInParent<Button>();

        menuActions.Add("GAME", () => { textTvUp.text = "Tutorial"; textTvDown.text = "Start Game"; tvDownButton.enabled = true; tvUpButton.onClick.AddListener(StartGame); tvDownButton.onClick.AddListener(StartTutorial); });
        menuActions.Add("OPTIONS", () => { textTvUp.text = "SFX Volume"; textTvDown.text = "Music Volume"; tvDownButton.enabled = true; tvUpButton.onClick.AddListener(ShowSfx); tvDownButton.onClick.AddListener(ShowMusic); });
        menuActions.Add("EXIT", () => { textTvUp.text = "Quit Game"; tvDownButton.enabled = false; tvUpButton.onClick.AddListener(Exitgame); });
        menuActions.Add("CREDITS", () => { textTvUp.text = "Team"; textTvDown.text = "Organization"; tvDownButton.enabled = true; tvUpButton.onClick.AddListener(ShowTeam); tvDownButton.onClick.AddListener(ShowEnti); });
                
        EventSystem.current.SetSelectedGameObject(firstSelectedObejct);
    }

    void Update()
    {
        if (!started) return;

        
        if(EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedObejct);
        }

        Debug.Log(EventSystem.current.currentSelectedGameObject.gameObject.name);

        if (EventSystem.current.currentSelectedGameObject.gameObject == tvUpButton.gameObject || EventSystem.current.currentSelectedGameObject.gameObject == tvDownButton.gameObject)
            bigTvSliderTexts.ToList().ForEach(tmp => tmp.color = customGrey);
        else
            bigTvSliderTexts.ToList().ForEach(tmp => tmp.color = tmp.gameObject.activeInHierarchy ? customRed : customGrey);


        int index = -1;
        float maxAngle = -1f; // Inicializa el �ngulo m�ximo con un valor m�nimo

        foreach (var text in pyramidMenuTexts)
        {
            // Calcula el vector de direcci�n del objeto dado al objeto actual
            Vector3 directionToText = -text.transform.forward;

            // Calcula el �ngulo entre el vector de direcci�n de la c�mara y el vector de direcci�n del objeto
            float angle = Vector3.Angle(cam.forward, directionToText);

            // Si el �ngulo es menor que el �ngulo m�ximo registrado, actualiza el m�ximo y el �ndice
            if (angle > maxAngle)
            {
                maxAngle = angle;
                index = pyramidMenuTexts.IndexOf(text);
            }
        }

        if (index != lastIndex)
        {
            pyramidMenuTexts[index].DOColor(Color.yellow, 0.8f).SetEase(Ease.InOutSine);
            if (lastIndex != -1)
                pyramidMenuTexts[lastIndex].DOColor(Color.white, 0.8f).SetEase(Ease.InOutSine);

            ResetTvs();
            menuActions[pyramidMenuTexts[index].text].Invoke();
            RotatitionDone();
        }

        lastIndex = index;
    }
}
