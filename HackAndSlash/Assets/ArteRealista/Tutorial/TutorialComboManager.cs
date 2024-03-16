using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialComboManager : MonoBehaviour
{
    [SerializeField] List<MultipleListElement<Enums.TutorialActions, Sprite>> mapActionWithSprite;
    [SerializeField] List<Image> uiImages;
    [SerializeField] Sprite emptySprite;
    [SerializeField] List<ListWrapper<Combo>> tutorialCombos;

    int currentPhase = 0, currentCombo = 0;

    public Action OnTutorialComplete;
    public Action OnPhaseComplete;
    protected void TutorialComplete() => OnTutorialComplete?.Invoke(); //Aqui es ficara la funció de color a verd
    protected void PhaseComplete() => OnPhaseComplete?.Invoke(); //Aqui es ficara la funció de color a verd
    public void StartCurrentCombo() => StartCoroutine(StartCombo());    
    void CurrentComboFailed() => StartCoroutine(tutorialCombos[currentPhase].collection[currentCombo].RestartCombo());
    void Awake()
    {        
        tutorialCombos.ForEach(lw => lw.collection.ForEach(c =>
        {
            c.Initialize(uiImages.GetRange(0, c.ComboSize), mapActionWithSprite);
            c.OnComboDone += CurrentComboDone;
            c.OnComboFailed += CurrentComboFailed;
        }));
    }
    void CurrentComboDone()
    {
        if (currentCombo == tutorialCombos[currentPhase].collection.Count - 1)
        {
            if (currentPhase == tutorialCombos.Count - 1)
            {
                currentCombo = 0;
                currentPhase = 0;
                TutorialComplete();
                return;
            }
            else
            {
                currentCombo = 0;
                PhaseComplete();
                return;
            }

        }
        else
            currentCombo++;

        StartCurrentCombo();            
    }
    IEnumerator StartCombo()
    { 
        yield return StartCoroutine(tutorialCombos[currentPhase].collection[currentCombo].ResetImages());

        uiImages.ForEach(image => { image.sprite = emptySprite; image.enabled = false; image.gameObject.SetActive(false); });        

        tutorialCombos[currentPhase].collection[currentCombo].LoadImages();

        tutorialCombos[currentPhase].collection[currentCombo].StartCombo();
    }

    
}
