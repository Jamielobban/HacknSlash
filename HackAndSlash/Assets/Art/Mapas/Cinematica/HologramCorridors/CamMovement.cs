using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

public class CamMovement : MonoBehaviour
{
    Sequence sequence;
    [SerializeField] List<TextMeshProUGUI> pyramidMenuTexts;
    int lastIndex = -1;
    // Start is called before the first frame update
    bool started = false;
    void StartShowMenu() { started = true;  pyramidMenuTexts.ForEach(text => { Color c = text.color; text.DOColor(new Color(c.r, c.g, c.b, 1), 1.5f).SetEase(Ease.InExpo); }); }

    void Start()
    {
        Tween moveTween = this.transform.DOMoveX(15.46f, 20).SetEase(Ease.InOutSine); //3.79f
        
        sequence = DOTween.Sequence();
        sequence.Append(moveTween)/*.Append(rotLeftTween).Append(rotRightTween).Append(rotStraightTween).Append(moveTween2)*/;
    }

    private void Update()
    {
        if (!started) return;

        int index = -1;
        float maxAngle = -1f; // Inicializa el ángulo máximo con un valor mínimo

        foreach (var text in pyramidMenuTexts)
        {
            // Calcula el vector de dirección del objeto dado al objeto actual
            Vector3 directionToText = -text.transform.forward;

            // Calcula el ángulo entre el vector de dirección de la cámara y el vector de dirección del objeto
            float angle = Vector3.Angle(transform.forward, directionToText);

            // Si el ángulo es menor que el ángulo máximo registrado, actualiza el máximo y el índice
            if (angle > maxAngle)
            {
                maxAngle = angle;
                index = pyramidMenuTexts.IndexOf(text);
            }
        }

        if (pyramidMenuTexts[index].color == Color.white)        
            pyramidMenuTexts[index].DOColor(Color.yellow, 0.8f).SetEase(Ease.InOutSine);

        if (index != lastIndex && lastIndex != -1)
            pyramidMenuTexts[lastIndex].DOColor(Color.white, 0.8f).SetEase(Ease.InOutSine);

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
