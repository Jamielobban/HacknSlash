using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using static Enums;
using System.Collections;

[System.Serializable]
public class Combo 
{
    public int ComboSize => comboActions.Count;
    public Action OnComboDone;
    public Action OnComboFailed;
    int currentIndex = 0;

    [SerializeField] List<Enums.TutorialActions> comboActions;
    List<ComboInput> comboInputs = new List<ComboInput>();
    List<Image> _comboImages;

    protected void ComboDone() => OnComboDone?.Invoke(); //Aqui es ficara la funció de color a verd
    protected void ComboFailed() => OnComboFailed?.Invoke();//Aqui es ficara la funció de color a vermell
    public void StartCombo() => comboInputs[0].StartListening(true); 
    public void LoadImages() => comboInputs.ForEach(ci => ci.LoadImage());
    public void Initialize(List<Image> comboImages, List<MultipleListElement<Enums.TutorialActions, Sprite>> mapActionWithSprite)
    {
        _comboImages = comboImages;
        
        for (int i = 0; i < comboActions.Count; i++)
        {
            comboInputs.Add(ParseComboActionToComboInput(comboActions[i], comboImages[i], mapActionWithSprite));
            comboInputs.Last().OnInputFailed += StepFailed;
            comboInputs.Last().OnInputDone += StepCompleted;
        }
    }
    void StepCompleted()
    {
        comboInputs[currentIndex].EndListening();

        if( currentIndex == comboActions.Count - 1)
            ComboDone();
        else
        {
            currentIndex++;
            comboInputs[currentIndex].StartListening(false);
        }
    }
    void StepFailed()
    {
        comboInputs[currentIndex].EndListening();
        currentIndex = 0;
        ComboFailed();
    }
    public IEnumerator ResetImages()
    {

        _comboImages.ForEach(image => DOVirtual.Color(image.color, Color.white, 0.80f, (col) => { image.color = col; }).SetEase(Ease.InOutSine));        

        yield return new WaitForSeconds(0.8f);

        Color color = Color.white;
        color.a = 0;
        _comboImages.ForEach(image => DOVirtual.Color(image.color, color, 0.5f, (col) => { image.color = col; }).SetEase(Ease.InOutSine));        

        yield return new WaitForSeconds(1f);        
    }
    public IEnumerator RestartCombo()
    {
        //AudioManager.Instance.PlayDelayFx(Effects.ErrorButton, 0);

        yield return new WaitForSeconds(1.3f);

        //AudioManager.Instance.PlayDelayFx(Enums.Effects.Negativo, 0.5f);
        _comboImages.ForEach(image => DOVirtual.Color(image.color, Color.red, 0.80f, (col) => { image.color = col; }).SetEase(Ease.InOutSine));        

        yield return new WaitForSeconds(0.8f);

        Color color = Color.white;
        color.a = 0;
        _comboImages.ForEach(image => DOVirtual.Color(image.color, color, 0.5f, (col) => { image.color = col; }).SetEase(Ease.InOutSine));        

        yield return new WaitForSeconds(1f);

        _comboImages.ForEach(image => DOVirtual.Color(image.color, Color.white, 0.8f, (col) => { image.color = col; }).SetEase(Ease.InOutSine));        
        StartCombo();
    }    
    ComboInput ParseComboActionToComboInput(Enums.TutorialActions cAction, Image uiIMage, List<MultipleListElement<Enums.TutorialActions, Sprite>> mapActionWithSprite)
    {
        switch (cAction)
        {
            case Enums.TutorialActions.JUMP:
                return new JumpCI(uiIMage, mapActionWithSprite.Where(e => e.key == TutorialActions.JUMP).Select(e => e.value).FirstOrDefault());
            case Enums.TutorialActions.DOBLEJUMP:
                return new DoubleJumpCI(uiIMage, mapActionWithSprite.Where(e => e.key == TutorialActions.DOBLEJUMP).Select(e => e.value).FirstOrDefault());
            case Enums.TutorialActions.RUN:
                return new RunCI(uiIMage, mapActionWithSprite.Where(e => e.key == TutorialActions.RUN).Select(e => e.value).FirstOrDefault());
            case Enums.TutorialActions.ROLL:
                return new RollCI(uiIMage, mapActionWithSprite.Where(e => e.key == TutorialActions.ROLL).Select(e => e.value).FirstOrDefault());
            case Enums.TutorialActions.SQUARE:
                return new SquareCI(uiIMage, mapActionWithSprite.Where(e => e.key == TutorialActions.SQUARE).Select(e => e.value).FirstOrDefault());
            case Enums.TutorialActions.TRIANGLE:
                return new TriangleCI(uiIMage, mapActionWithSprite.Where(e => e.key == TutorialActions.TRIANGLE).Select(e => e.value).FirstOrDefault());
            case Enums.TutorialActions.HOLDSQUARE:
                return new HoldSquareCI(uiIMage, mapActionWithSprite.Where(e => e.key == TutorialActions.HOLDSQUARE).Select(e => e.value).FirstOrDefault());
            case Enums.TutorialActions.HOLDTRIANGLE:
                return new HoldTriangleCI(uiIMage, mapActionWithSprite.Where(e => e.key == TutorialActions.HOLDTRIANGLE).Select(e => e.value).FirstOrDefault());
            case Enums.TutorialActions.AIRSQUARE:
                return new AirSquareCI(uiIMage, mapActionWithSprite.Where(e => e.key == TutorialActions.AIRSQUARE).Select(e => e.value).FirstOrDefault());
            case Enums.TutorialActions.AIRTRIANGLE:
                return new AirTriangleCI(uiIMage, mapActionWithSprite.Where(e => e.key == TutorialActions.AIRTRIANGLE).Select(e => e.value).FirstOrDefault());
            default:
                return null;
        }
    }
}
