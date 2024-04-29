using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> pyramidMenuTexts;
    [SerializeField] TextMeshProUGUI textTvUp;
    [SerializeField] TextMeshProUGUI textTvDown;
    [SerializeField] RectTransform bigTvCanvas;
    Button tvUpButton;
    Button tvDownButton;

    Dictionary<string, Action> menuActions = new Dictionary<string, Action>();
    int lastIndex = -1;
    bool started = false;

    Transform cam;

    public void MainMenuEnter() => Invoke(nameof(StartShowMenu), 0.5f);
    void StartShowMenu() { started = true; pyramidMenuTexts.ForEach(text => { Color c = text.color; text.DOColor(new Color(c.r, c.g, c.b, 1), 1.5f).SetEase(Ease.InExpo); }); }
    void NewGame() => Debug.Log("New Game");
    void LoadGame() => Debug.Log("Load Game");
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

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        tvUpButton = textTvUp.GetComponentInParent<Button>();
        tvDownButton = textTvDown.GetComponentInParent<Button>();

        menuActions.Add("GAME", () => { textTvUp.text = "New Game"; textTvDown.text = "Load Game"; tvUpButton.onClick.AddListener(NewGame); tvDownButton.onClick.AddListener(LoadGame); });
        menuActions.Add("OPTIONS", () => { textTvUp.text = "SFX Volume"; textTvDown.text = "Music Volume"; tvUpButton.onClick.AddListener(ShowSfx); tvDownButton.onClick.AddListener(ShowMusic); });
        menuActions.Add("EXIT", () => { textTvUp.text = "Quit Game"; tvUpButton.onClick.AddListener(Exitgame); });
        menuActions.Add("CREDITS", () => { textTvUp.text = "Team"; textTvDown.text = "Organization"; tvUpButton.onClick.AddListener(ShowTeam); tvDownButton.onClick.AddListener(ShowEnti); });

    }

    // Update is called once per frame
    void Update()
    {
        if (!started) return;

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

        //if (pyramidMenuTexts[index].color == Color.white)        
        //    pyramidMenuTexts[index].DOColor(Color.yellow, 0.8f).SetEase(Ease.InOutSine);

        if (index != lastIndex)
        {
            pyramidMenuTexts[index].DOColor(Color.yellow, 0.8f).SetEase(Ease.InOutSine);
            if (lastIndex != -1)
                pyramidMenuTexts[lastIndex].DOColor(Color.white, 0.8f).SetEase(Ease.InOutSine);

            ResetTvs();
            menuActions[pyramidMenuTexts[index].text].Invoke();
        }

        lastIndex = index;
    }
}
