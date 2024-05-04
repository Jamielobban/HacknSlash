using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public struct SpriteInputAllController
{
    [SerializeField]
    public Sprite playStation, xbox;
}

public class TutorialComboManager : MonoBehaviour
{ 
    [SerializeField] List<MultipleListElement<Enums.TutorialActions, SpriteInputAllController>> mapActionWithSprite;
    [SerializeField] List<ListWrapper<Combo>> tutorialCombos;
    [SerializeField] List<Image> uiImages;
    [SerializeField] Sprite emptySprite;

    Image panelImage;
    int currentPhase = 0, currentCombo = 0;

    public Action OnTutorialComboComplete;
    public Action OnCombosListComplete;
    protected void TutorialComboComplete() => OnTutorialComboComplete?.Invoke(); //Aqui es ficara la funció de color a verd
    protected void CombosListComplete() => OnCombosListComplete?.Invoke(); //Aqui es ficara la funció de color a verd
    void CurrentComboFailed() => StartCoroutine(tutorialCombos[currentPhase].collection[currentCombo].RestartCombo());
    void Awake()
    {
        panelImage = uiImages[0].transform.parent.GetComponent<Image>();
        tutorialCombos.ForEach(lw => lw.collection.ForEach(c =>
        {
            c.Initialize(uiImages.GetRange(0, c.ComboSize), mapActionWithSprite);
            c.OnComboDone += CurrentComboDone;
            c.OnComboFailed += CurrentComboFailed;
        }));
    }
    void CurrentComboDone()
    {
        if (currentCombo == tutorialCombos[currentPhase].collection.Count - 1) //Si era ultim combo de la phase
        {
            currentCombo = 0;
            StartCoroutine(EndCurrentCombosList()); //Acabem phase
            if (currentPhase == tutorialCombos.Count - 1) //Si era ultima phase
            {
                AudioManager.Instance.PlayDelayFx(Enums.Effects.TutoEnded, 1.3f);
                TutorialComboComplete(); //Acabem tutorial de combos                
            }
            else
                AudioManager.Instance.PlayDelayFx(Enums.Effects.Positivo, 0.4f);

            return;
        }

        AudioManager.Instance.PlayDelayFx(Enums.Effects.Positivo, 0.4f);
        StartCoroutine(StartCombo(true));
    }
    IEnumerator StartCombo(bool cameFromOtherCombo)
    {
        if(cameFromOtherCombo)
            yield return new WaitForSeconds(1);

        yield return StartCoroutine(tutorialCombos[currentPhase].collection[currentCombo].ResetImages());

        if(cameFromOtherCombo)
            currentCombo++;

        uiImages.ForEach(image => { image.sprite = emptySprite; image.enabled = false; image.gameObject.SetActive(false); });        

        tutorialCombos[currentPhase].collection[currentCombo].LoadImages();

        tutorialCombos[currentPhase].collection[currentCombo].StartCombo();
    }

    public void StartCurrentCombosList()
    {
        Image parentImage = uiImages[0].transform.parent.GetComponent<Image>();
        
        DOVirtual.Color(parentImage.color, new Color(1, 1, 1, 1), 1.5f, (color) =>
        {
            parentImage.color = color;
        }).SetEase(Ease.Linear);

        StartCoroutine(StartCombo(false));
    }

    public IEnumerator EndCurrentCombosList()
    {
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(tutorialCombos[currentPhase].collection[currentCombo].ResetImages());        

        DOVirtual.Color(panelImage.color, new Color(1, 1, 1, 0), 1f, (color) =>
        {
            panelImage.color = color;
        }).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1.1f);

        for (int i = 1; i < uiImages.Count - 1; i++)
        {
            uiImages[i].enabled = false;
            uiImages[i].gameObject.SetActive(false);
        }
        uiImages[0].sprite = emptySprite;
        uiImages[0].color = Color.white;

        if(currentPhase < tutorialCombos.Count - 1) 
            currentPhase++;

        CombosListComplete();
    }

    private void OnDestroy()
    {
        tutorialCombos.ForEach(lw => lw.collection.ForEach(c =>
        {
            c.Initialize(uiImages.GetRange(0, c.ComboSize), mapActionWithSprite);
            c.OnComboDone -= CurrentComboDone;
            c.OnComboFailed -= CurrentComboFailed;
        }));
    }
}
