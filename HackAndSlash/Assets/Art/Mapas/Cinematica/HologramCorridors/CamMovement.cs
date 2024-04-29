using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class CamMovement : MonoBehaviour
{
    [SerializeField] MainMenuManager mainMenuManager;
    Sequence sequence;

    //[SerializeField] List<TextMeshProUGUI> pyramidMenuTexts;
    //[SerializeField] TextMeshProUGUI textTvUp;
    //[SerializeField] TextMeshProUGUI textTvDown;
    //[SerializeField] RectTransform bigTvCanvas;
    //Button tvUpButton;
    //Button tvDownButton;

    //Dictionary<string, Action> menuActions = new Dictionary<string, Action>();
    //int lastIndex = -1;
    //bool started = false;

    //void StartShowMenu() { started = true;  pyramidMenuTexts.ForEach(text => { Color c = text.color; text.DOColor(new Color(c.r, c.g, c.b, 1), 1.5f).SetEase(Ease.InExpo); }); }
    //void NewGame() => Debug.Log("New Game");
    //void LoadGame() => Debug.Log("Load Game");
    //void Exitgame() => Application.Quit();
    //void ShowTeam() { bigTvCanvas.GetChild(0).gameObject.SetActive(true); bigTvCanvas.GetChild(1).gameObject.SetActive(false); Debug.Log("ShowTeam"); }
    //void ShowEnti() { bigTvCanvas.GetChild(0).gameObject.SetActive(false); bigTvCanvas.GetChild(1).gameObject.SetActive(true); Debug.Log("ShowEnti"); }
    //void ShowSfx() { bigTvCanvas.GetChild(2).gameObject.SetActive(true); bigTvCanvas.GetChild(3).gameObject.SetActive(false); Debug.Log("Sfx"); }
    //void ShowMusic() { bigTvCanvas.GetChild(2).gameObject.SetActive(false); bigTvCanvas.GetChild(3).gameObject.SetActive(true); Debug.Log("Music"); }
    //void ResetTvs() { tvUpButton.onClick.RemoveAllListeners(); tvDownButton.onClick.RemoveAllListeners(); textTvUp.text = ""; textTvDown.text = "";
    //    bigTvCanvas.GetComponentsInChildren<RectTransform>(false).Where(t => t != bigTvCanvas && t.parent == bigTvCanvas).ToList().ForEach(t => t.gameObject.SetActive(false));
    //}

    void Start()
    {
        //tvUpButton = textTvUp.GetComponentInParent<Button>();
        //tvDownButton = textTvDown.GetComponentInParent<Button>();

        //menuActions.Add("GAME", () => { textTvUp.text = "New Game"; textTvDown.text = "Load Game"; tvUpButton.onClick.AddListener(NewGame); tvDownButton.onClick.AddListener(LoadGame); });
        //menuActions.Add("OPTIONS", () => { textTvUp.text = "SFX Volume"; textTvDown.text = "Music Volume"; tvUpButton.onClick.AddListener(ShowSfx); tvDownButton.onClick.AddListener(ShowMusic); });
        //menuActions.Add("EXIT", () => { textTvUp.text = "Quit Game"; tvUpButton.onClick.AddListener(Exitgame); });
        //menuActions.Add("CREDITS", () => { textTvUp.text = "Team"; textTvDown.text = "Organization"; tvUpButton.onClick.AddListener(ShowTeam); tvDownButton.onClick.AddListener(ShowEnti); });

        Tween moveTween = this.transform.DOMoveX(15.46f, 20).SetEase(Ease.InOutSine); //3.79f
        
        sequence = DOTween.Sequence();
        sequence.Append(moveTween);
    }

    //private void Update()
    //{
    //    if (!started) return;

    //    int index = -1;
    //    float maxAngle = -1f; // Inicializa el ángulo máximo con un valor mínimo

    //    foreach (var text in pyramidMenuTexts)
    //    {
    //        // Calcula el vector de dirección del objeto dado al objeto actual
    //        Vector3 directionToText = -text.transform.forward;

    //        // Calcula el ángulo entre el vector de dirección de la cámara y el vector de dirección del objeto
    //        float angle = Vector3.Angle(transform.forward, directionToText);

    //        // Si el ángulo es menor que el ángulo máximo registrado, actualiza el máximo y el índice
    //        if (angle > maxAngle)
    //        {
    //            maxAngle = angle;
    //            index = pyramidMenuTexts.IndexOf(text);
    //        }
    //    }

    //    //if (pyramidMenuTexts[index].color == Color.white)        
    //    //    pyramidMenuTexts[index].DOColor(Color.yellow, 0.8f).SetEase(Ease.InOutSine);

    //    if (index != lastIndex)
    //    {
    //        pyramidMenuTexts[index].DOColor(Color.yellow, 0.8f).SetEase(Ease.InOutSine);
    //        if(lastIndex != -1)
    //            pyramidMenuTexts[lastIndex].DOColor(Color.white, 0.8f).SetEase(Ease.InOutSine);
            
    //        ResetTvs();
    //        menuActions[pyramidMenuTexts[index].text].Invoke();
    //    }

    //    lastIndex = index;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Animator>(out Animator anim))
        {
            if (this.transform.position.x > 6)             
                mainMenuManager.MainMenuEnter();
                            
            anim.CrossFadeInFixedTime("DoorOpen", 0.2f);
        }
    }
}
