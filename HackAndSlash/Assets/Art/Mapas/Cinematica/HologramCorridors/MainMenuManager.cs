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
    [SerializeField] BlackCyborg bc;
    [SerializeField] SpinPyramid spinPyramid;
    [SerializeField] Image blackImage;
    [SerializeField] List<TextMeshProUGUI> pyramidMenuTexts;
    [SerializeField] TextMeshProUGUI textTvUp;
    [SerializeField] TextMeshProUGUI textTvDown;
    [SerializeField] RectTransform bigTvCanvas;
    [SerializeField] TextMeshProUGUI[] bigTvSliderTexts;
    [SerializeField] Color32 customGrey;
    [SerializeField] Color32 customRed;

    FadeScript fadeScript;
    Button tvUpButton;
    Button tvDownButton;

    MenuInputs menuInputs;
    
    Dictionary<string, Action> menuActions = new Dictionary<string, Action>();
    int lastIndex = -1;
    bool started = false;
    private GameObject lastSelectedObj;

    Transform cam;

    public GameObject firstSelectedObejct;

    public GameObject loadingGo;
    public Image fillLoadingGo;
    bool rotated = false, firstTime = true;

    
    public void MainMenuEnter() => Invoke(nameof(StartShowMenu), 0.5f);
    public void RotatitionDone() {
        rotated = true;
        AudioManager.Instance.PlayFx(Enums.Effects.RotatePyramid);
        EventSystem.current.SetSelectedGameObject(firstSelectedObejct); 
    }
    void StartNarrationAudio() => AudioManager.Instance.PlayFx(Enums.Effects.MenuSpeach);
    void StartNarrationText() { bc.Speak(0); Invoke(nameof(StartNarrationAudio), 0.5f); }
    void StartShowMenu() {
        AudioManager.Instance.PlayMusicEffect(Enums.MusicEffects.Glitch);
        pyramidMenuTexts.ForEach(text => { Color c = text.color; text.DOColor(text.text == "GAME" ? Color.yellow : new Color(c.r, c.g, c.b, 1), 1.5f).SetEase(Ease.InExpo).OnComplete(() => { started = true; if (pyramidMenuTexts.IndexOf(text) == pyramidMenuTexts.Count - 1) spinPyramid.StartListening(); }); }); }
    void StartGame()  
    {        
        GameManager.Instance.isTutorialCompleted = true;
        GameManager.Instance.LoadLevel(Constants.SCENE_MAIN, fillLoadingGo);
    }
    void StartTutorial()
    {
        loadingGo.SetActive(true);
        GameManager.Instance.LoadLevel(Constants.SCENE_TUTORIAL_CINEMATIC, fillLoadingGo);
    }

    void Exitgame() { AudioManager.Instance.PlayFx(Enums.Effects.SelectOptionMenu); Application.Quit(); }
    void ShowTeam() { AudioManager.Instance.PlayFx(Enums.Effects.SelectOptionMenu);
        bigTvCanvas.GetChild(0).gameObject.SetActive(true); bigTvCanvas.GetChild(1).gameObject.SetActive(false); Debug.Log("ShowTeam"); }
    void ShowTeamMuted()
    {
        bigTvCanvas.GetChild(0).gameObject.SetActive(true); bigTvCanvas.GetChild(1).gameObject.SetActive(false); Debug.Log("ShowTeam");
    }
    void ShowEnti() { AudioManager.Instance.PlayFx(Enums.Effects.SelectOptionMenu);
        bigTvCanvas.GetChild(0).gameObject.SetActive(false); bigTvCanvas.GetChild(1).gameObject.SetActive(true); Debug.Log("ShowEnti"); }
    void ShowEntiMuted()
    {
        bigTvCanvas.GetChild(0).gameObject.SetActive(false); bigTvCanvas.GetChild(1).gameObject.SetActive(true); Debug.Log("ShowEnti");
    }
    void ShowSfx() { AudioManager.Instance.PlayFx(Enums.Effects.SelectOptionMenu);
        bigTvCanvas.GetChild(2).gameObject.SetActive(true); bigTvCanvas.GetChild(3).gameObject.SetActive(false); Debug.Log("Sfx"); }
    void ShowSfxMuted()
    {
        bigTvCanvas.GetChild(2).gameObject.SetActive(true); bigTvCanvas.GetChild(3).gameObject.SetActive(false); Debug.Log("Sfx");
    }
    void ShowMusic() { AudioManager.Instance.PlayFx(Enums.Effects.SelectOptionMenu);
        bigTvCanvas.GetChild(2).gameObject.SetActive(false); bigTvCanvas.GetChild(3).gameObject.SetActive(true); Debug.Log("Music"); }
    void ShowMusicMuted()
    {
        bigTvCanvas.GetChild(2).gameObject.SetActive(false); bigTvCanvas.GetChild(3).gameObject.SetActive(true); Debug.Log("Music");
    }
    void ResetTvs()
    {
        tvUpButton.onClick.RemoveAllListeners(); tvDownButton.onClick.RemoveAllListeners(); textTvUp.text = ""; textTvDown.text = "";
        bigTvCanvas.GetComponentsInChildren<RectTransform>(false).Where(t => t != bigTvCanvas && t.parent == bigTvCanvas).ToList().ForEach(t => t.gameObject.SetActive(false));
    }

    public void ButtonHiglighted(GameObject selectedObject)
    {
        Debug.Log("EnterFunc");
        TextMeshProUGUI tmpText = selectedObject.GetComponentInChildren<TextMeshProUGUI>();

        if (tmpText == null) return;

        if (tmpText.text == "Team")
            ShowTeam();
        else if(tmpText.text == "SFX Volume")
            ShowSfx();
        else if (tmpText.text == "Organization")
            ShowEnti();
        else if(tmpText.text == "Music Volume")
            ShowMusic();
    }

    void Start()
    {
        GameManager.Instance.UpdateState(Enums.GameState.Menu);
        fadeScript = new FadeScript(blackImage);
        fadeScript.FadeIn(1);
        AudioManager.Instance.PlayMusic(Enums.Music.MusicaMenuNuevo);
        Invoke(nameof(StartNarrationText), 1.85f);
        cam = Camera.main.transform;
        tvUpButton = textTvUp.GetComponentInParent<Button>();
        tvDownButton = textTvDown.GetComponentInParent<Button>();
        menuActions.Add("GAME", () => { textTvUp.text = "Tutorial"; tvUpButton.onClick.AddListener(StartTutorial);
            tvDownButton.enabled = false;
            if (GameManager.Instance.isTutorialCompleted)
            {
                textTvDown.text = "Start Game";
                tvDownButton.enabled = true;
                tvDownButton.onClick.AddListener(StartGame);
            }
        });
        menuActions.Add("OPTIONS", () => { textTvUp.text = "SFX Volume"; textTvDown.text = "Music Volume"; tvDownButton.enabled = true; tvUpButton.onClick.AddListener(ShowSfxMuted); tvDownButton.onClick.AddListener(ShowMusicMuted); ShowSfx(); });
        menuActions.Add("EXIT", () => { textTvUp.text = "Quit Game"; tvDownButton.enabled = false; tvUpButton.onClick.AddListener(Exitgame); });
        menuActions.Add("CREDITS", () => { textTvUp.text = "Team"; textTvDown.text = "Organization"; tvDownButton.enabled = true; tvUpButton.onClick.AddListener(ShowTeamMuted); tvDownButton.onClick.AddListener(ShowEntiMuted); ShowTeam(); });
                
        EventSystem.current.SetSelectedGameObject(firstSelectedObejct);
        lastSelectedObj = firstSelectedObejct;      
        lastIndex = 0;        
    }

    void Update()
    {
        if (!started) return;

        if(Input.GetKeyDown(KeyCode.L) && !tvDownButton.enabled)
        {
            textTvDown.text = "Start Game";
            tvDownButton.enabled = true;
            tvDownButton.onClick.AddListener(StartGame);
        }

        if(EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedObejct);
        }

        //Debug.Log(EventSystem.current.currentSelectedGameObject.gameObject.name);

        if (EventSystem.current.currentSelectedGameObject.gameObject == tvUpButton.gameObject || EventSystem.current.currentSelectedGameObject.gameObject == tvDownButton.gameObject)
            bigTvSliderTexts.ToList().ForEach(tmp => tmp.color = customGrey);
        else
            bigTvSliderTexts.ToList().ForEach(tmp => tmp.color = tmp.gameObject.activeInHierarchy ? customRed : customGrey);


        int index = -1;
        float maxAngle = -1f; // Inicializa el ángulo máximo con un valor mínimo

        foreach (var text in pyramidMenuTexts)
        {
            // Calcula el vector de dirección del objeto dado al objeto actual
            Vector3 directionToText = -text.transform.forward;

            // Calcula el ángulo entre el vector de dirección de la cámara y el vector de dirección del objeto
            float angle = Vector3.Angle(cam.forward, directionToText);

            // Si el ángulo es menor que el ángulo máximo registrado, actualiza el máximo y el índice
            if (angle > maxAngle)
            {
                maxAngle = angle;
                index = pyramidMenuTexts.IndexOf(text);
            }
        }

        if (index != lastIndex || firstTime)
        {
            pyramidMenuTexts[index].DOColor(Color.yellow, 0.8f).SetEase(Ease.InOutSine);
            if (lastIndex != -1 && !firstTime)
                pyramidMenuTexts[lastIndex].DOColor(Color.white, 0.8f).SetEase(Ease.InOutSine);

            ResetTvs();
            menuActions[pyramidMenuTexts[index].text].Invoke();
            RotatitionDone();
        }

        lastIndex = index;

        if(EventSystem.current.currentSelectedGameObject != lastSelectedObj && EventSystem.current.currentSelectedGameObject.GetComponent<Slider>()==null)
        {
            ButtonHiglighted(EventSystem.current.currentSelectedGameObject);
            if(!rotated)
                AudioManager.Instance.PlayFx(Enums.Effects.MoveUI);
        }
        lastSelectedObj = EventSystem.current.currentSelectedGameObject;
        rotated = false;

        firstTime = false;
    }    
}
