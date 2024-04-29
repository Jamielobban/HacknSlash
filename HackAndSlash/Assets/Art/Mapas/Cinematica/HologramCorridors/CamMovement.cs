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
    Sequence sequence;
    [SerializeField] List<TextMeshProUGUI> pyramidMenuTexts;
    [SerializeField] TextMeshProUGUI textTvUp;
    [SerializeField] TextMeshProUGUI textTvDown;
    Button tvUpButton;
    Button tvDownButton;

    Dictionary<string, Action> menuActions = new Dictionary<string, Action>();
    int lastIndex = -1;
    bool started = false;

    void StartShowMenu() { started = true;  pyramidMenuTexts.ForEach(text => { Color c = text.color; text.DOColor(new Color(c.r, c.g, c.b, 1), 1.5f).SetEase(Ease.InExpo); }); }
    void NewGame() => Debug.Log("New Game");
    void LoadGame() => Debug.Log("Load Game");
    void Exitgame() => Application.Quit();
    void ResetTvs() { tvUpButton.onClick.RemoveAllListeners(); tvDownButton.onClick.RemoveAllListeners(); textTvUp.text = ""; textTvDown.text = ""; }

    void Start()
    {
        tvUpButton = textTvUp.GetComponentInParent<Button>();
        tvDownButton = textTvDown.GetComponentInParent<Button>();

        menuActions.Add("GAME", () => { textTvUp.text = "New Game"; textTvDown.text = "Load Game"; tvUpButton.onClick.AddListener(NewGame); tvDownButton.onClick.AddListener(LoadGame); });
        menuActions.Add("OPTIONS", () => { });
        menuActions.Add("EXIT", () => { textTvUp.text = "Quit Game"; tvUpButton.onClick.AddListener(Exitgame); });
        menuActions.Add("CREDITS", () => { });

        Tween moveTween = this.transform.DOMoveX(15.46f, 20).SetEase(Ease.InOutSine); //3.79f
        
        sequence = DOTween.Sequence();
        sequence.Append(moveTween);
    }

    private void Update()
    {
        if (!started) return;

        int index = -1;
        float maxAngle = -1f; // Inicializa el �ngulo m�ximo con un valor m�nimo

        foreach (var text in pyramidMenuTexts)
        {
            // Calcula el vector de direcci�n del objeto dado al objeto actual
            Vector3 directionToText = -text.transform.forward;

            // Calcula el �ngulo entre el vector de direcci�n de la c�mara y el vector de direcci�n del objeto
            float angle = Vector3.Angle(transform.forward, directionToText);

            // Si el �ngulo es menor que el �ngulo m�ximo registrado, actualiza el m�ximo y el �ndice
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
            if(lastIndex != -1)
                pyramidMenuTexts[lastIndex].DOColor(Color.white, 0.8f).SetEase(Ease.InOutSine);
            
            ResetTvs();
            menuActions[pyramidMenuTexts[index].text].Invoke();
        }

        lastIndex = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Animator>(out Animator anim))
        {
            if (this.transform.position.x > 6)             
                Invoke(nameof(StartShowMenu), 0.5f);
                            
            anim.CrossFadeInFixedTime("DoorOpen", 0.2f);
        }
    }
}
