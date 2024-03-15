using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using static Enums;
using System.Collections;

public class Combo : MonoBehaviour 
{
    [SerializeField] List<Enums.TutorialActions> comboActions;

    List<ComboInput> comboInputs = new List<ComboInput>();
    List<Image> _comboImages;

    int currentIndex = 0;


    public Action OnComboDone;
    public Action OnComboFailed;
    protected void ComboDone() => OnComboDone?.Invoke(); //Aqui es ficara la funció de color a verd
    protected void ComboFailed() => OnComboFailed?.Invoke();//Aqui es ficara la funció de color a vermell

    public void Initialize(List<Image> comboImages)
    {
        _comboImages = comboImages;
        
        for (int i = 0; i < comboActions.Count; i++)
        {
            comboInputs.Add(ParseComboActionToComboInput(comboActions[i], comboImages[i]));
            comboInputs.Last().OnInputFailed += ComboFailed;
            comboInputs.Last().OnInputDone += StepCompleted;
        }
    }

    void StepCompleted()
    {
        if( currentIndex == comboActions.Count - 1)
            ComboDone();
        else
            currentIndex++;
    }    

    //IEnumerator RestartCombo()
    //{
    //    AudioManager.Instance.PlayDelayFx(Effects.ErrorButton, 0);
    //    yield return new WaitForSeconds(1.3f);

    //    AudioManager.Instance.PlayDelayFx(Enums.Effects.Negativo, 0.5f);
    //    foreach (Image image in _comboImages)
    //    {
    //        DOVirtual.Color(image.color, Color.red, 0.80f, (col) =>
    //        {
    //            image.color = col;
    //        }).SetEase(Ease.InOutSine);

    //    }

    //    yield return new WaitForSeconds(0.8f);

    //    Color color = Color.white;
    //    color.a = 0;

    //    foreach (Image image in _comboImages)
    //    {
    //        DOVirtual.Color(image.color, color, 0.5f, (col) =>
    //        {
    //            image.color = col;
    //        }).SetEase(Ease.InOutSine);

    //    }

    //    yield return new WaitForSeconds(1f);

    //    foreach (Image image in _comboImages)
    //    {
    //        DOVirtual.Color(image.color, Color.white, 0.80f, (col) =>
    //        {
    //            image.color = col;
    //        }).SetEase(Ease.InOutSine);

    //    }

    //}


    //IEnumerator StartPhase(Enums.TutorialState newPhase, int imagesNeeded, System.Action onFinishCurrentAction, System.Action onStartNewAction, float delayOnEnter)
    //{
    //    ChangeTutorialPhase(newPhase);

    //    onFinishCurrentAction();

    //    if ((newPhase != TutorialState.MOVEMENTS || (newPhase == TutorialState.MOVEMENTS && currentInputsListIndex != 0)) && (newPhase != TutorialState.COMBOS || (newPhase == TutorialState.COMBOS && currentInputsListIndex != 0)))
    //        AudioManager.Instance.PlayDelayFx(Enums.Effects.Positivo, 0.4f);

    //    yield return new WaitForSeconds(delayOnEnter);

    //    yield return StartCoroutine(ResetImages());

    //    for (int i = 0; i < 4; i++)
    //    {
    //        uiCombosImage[i].sprite = emptySprite;
    //        uiCombosImage[i].enabled = false;
    //        uiCombosImage[i].gameObject.SetActive(false);
    //    }

    //    //Appear new images, enabled only necessary ones and transparent to 1

    //    for (int i = 0; i < 4; i++)
    //    {
    //        if (i <= imagesNeeded - 1)
    //        {
    //            uiCombosImage[i].sprite = matchKeyWithUI.Where(e => e.key == tutorialPhasesInputs[(int)currentTutorialGeneralPhase].collection[currentInputsListIndex].collection[i]).Select(e => e.value).First();

    //            if (!uiCombosImage[i].enabled)
    //                uiCombosImage[i].enabled = true;

    //            if (!uiCombosImage[i].gameObject.activeSelf)
    //                uiCombosImage[i].gameObject.SetActive(true);

    //            int ind = i;

    //            DOVirtual.Color(uiCombosImage[ind].color, Color.white, 0.6f, (col) =>
    //            {
    //                uiCombosImage[ind].color = col;
    //            }).SetEase(Ease.InOutSine);

    //            //yield return new WaitForSeconds(0.6f);
    //        }

    //    }

    //    onStartNewAction();

    //}



    ComboInput ParseComboActionToComboInput(Enums.TutorialActions cAction, Image uiIMage)
    {
        switch (cAction)
        {
            case Enums.TutorialActions.JUMP:
                return new JumpCI(uiIMage);
            case Enums.TutorialActions.DOBLEJUMP:
                return new DoubleJumpCI(uiIMage);
            case Enums.TutorialActions.RUN:
                return new RunCI(uiIMage);
            case Enums.TutorialActions.ROLL:
                return new RollCI(uiIMage);
            case Enums.TutorialActions.SQUARE:
                return new SquareCI(uiIMage);
            case Enums.TutorialActions.TRIANGLE:
                return new TriangleCI(uiIMage);
            case Enums.TutorialActions.HOLDSQUARE:
                return new HoldSquareCI(uiIMage);
            case Enums.TutorialActions.HOLDTRIANGLE:
                return new HoldTriangleCI(uiIMage);
            case Enums.TutorialActions.AIRSQUARE:
                return new AirSquareCI(uiIMage);
            case Enums.TutorialActions.AIRTRIANGLE:
                return new AirTriangleCI(uiIMage);
            default:
                return null;
        }
    }
}
