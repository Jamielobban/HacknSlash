using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialComboManager : MonoBehaviour
{
    List<ListWrapper<Combo>> tutorialCombos;
    [SerializeField] List<MultipleListElement<Enums.TutorialActions, Sprite>> mapActionWithSprite;
    [SerializeField] List<Image> uiImages;

    int currentPhase = 0, currentCombo = 0;

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void StartTutorial()
    {

    }

    
}
