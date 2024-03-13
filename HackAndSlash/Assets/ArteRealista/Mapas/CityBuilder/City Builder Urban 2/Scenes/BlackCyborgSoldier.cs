using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;
using static Enums;

public class BlackCyborgSoldier : Interactive, IInteractable
{
    //DIALOGUE 
    [TextArea]
    [SerializeField] string[] dialogues;

    [SerializeField] List<ListWrapper<ListWrapper<Enums.TutorialInputs>>> tutorialPhasesInputs;
    [SerializeField] List<MultipleListElement<TutorialInputs, Sprite>> matchKeyWithUI;    
    [SerializeField] SimpleRTVoiceExample voice;
    [SerializeField] bool intro, hasSprinted = false, walking = false;
    [SerializeField] Image[] uiCombosImage = new Image[4];
    [SerializeField] Sprite emptySprite;
    [SerializeField] GameObject enemyHealthUI;
    [SerializeField] BoxCollider enemyDamageableCol;     
    [SerializeField] GameObject pyramidTracker;

    public GameObject objectiveMarker;
    readonly string name = "Cyborg Sergeant";
    int currentText = 0, currentComboLevel = 0, currentAirComboLevel = 0, jumpsDone = -1, rollsDone = -1, lastBufferSize = 0;
    float lastAttackTime = 0, lastDoubleJumpTime = 0;
    bool _isL2Performed;
    List<TutorialInputs> inputsBuffer = new List<TutorialInputs>();
   
    Enums.TutorialState currentState = Enums.TutorialState.INACTIVE;
    [SerializeField] private float maxTimeBetweenCombosInputs = 0.8f;
    Inputs _playerActions;
    private PlayerControl _player;
    private void Awake()
    {
        if (intro)
            return;

        GameManager.Instance.UpdateState(Enums.GameState.Tutorial);
        _playerActions = new Inputs();
        _playerActions.Player.Enable();

    }

    private void Start()
    {
        if (intro)
        {
            StartCoroutine(IntroSpeach());
            return;
        }
        
        _player = FindObjectOfType<PlayerControl>();
    }


    private void Update()
    {
        //Debug.Log(voice.playing);

        if (intro)
            return;

        if (canInteract && currentState != Enums.TutorialState.FINISHED)
            objectiveMarker.SetActive(true);

        if (currentState == Enums.TutorialState.INACTIVE || currentState == Enums.TutorialState.FINISHED)
            return;
        /*
        if (currentState == TutorialState.DOUBLEJUMP)
        {
            if (jumpsDone >= 0 && Time.time - lastDoubleJumpTime >= 0.3f && _player.CheckIfLand())
            {
                jumpsDone = -1;
                _playerActions.Player.Jump.performed -= DoubleJump_performed;
                lastDoubleJumpTime = 0;
                DOVirtual.Color(uiCombosImage[1].color, Color.red, 1f, (color) =>
                {
                    uiCombosImage[1].color = color;
                }).SetEase(Ease.InOutSine);

                StartCoroutine(RestartCombo(() =>
                {
                    _playerActions.Player.Jump.performed += DoubleJump_performed;
                }));
            }
        }
        else */ if (currentState == Enums.TutorialState.COMBOS)
        {
            if (lastAttackTime == 0 || inputsBuffer.Count() > tutorialPhasesInputs[currentComboLevel].collection.Count() || inputsBuffer.Count == 0)
                return;

            if (Time.time - lastAttackTime >= maxTimeBetweenCombosInputs)
                ComboFailed(1);
            else
            {
                if (inputsBuffer.Count() <= 0 || lastBufferSize == inputsBuffer.Count() || lastAttackTime == 0)
                    return;

                //if (inputsBuffer.Last() != tutorialPhasesInputs[currentComboLevel].collection[inputsBuffer.Count() - 1])
                //{
                //    ComboFailed();
                //}
                //else
                //{
                //    StepDone();
                //    if (inputsBuffer.Count() == tutorialPhasesInputs[currentComboLevel].collection.Count())
                //        ComboCompleted();
                //}
            }

        }
       

        lastBufferSize = inputsBuffer.Count();
    }


    public void Interact()
    {
        if (!canInteract || intro || voice.playing) return;

        voice.Speak(dialogues[currentText], name);

        objectiveMarker.SetActive(false);

        Image parentImage = uiCombosImage[0].transform.parent.GetComponent<Image>();

        //if (currentState == Enums.TutorialState.INACTIVE)
        //    StartCoroutine(StartPhase(Enums.TutorialState.JUMP, 1,
        //    () => {
        //        canInteract = false;
        //        currentText++;
        //        DOVirtual.Color(parentImage.color, new Color(1, 1, 1, 1), 1.5f, (color) =>
        //        {
        //            parentImage.color = color;
        //        }).SetEase(Ease.Linear);
        //    },
        //    () => {
        //        _playerActions.Player.Jump.performed += Jump_performed;
        //    }, 0));
        //else if (currentState == Enums.TutorialState.ROLL)
        //    StartCoroutine(StartPhase(Enums.TutorialState.COMBOS, tutorialPhasesInputs[currentComboLevel].collection.Count(),
        //    () => {
        //        canInteract = false;
        //        currentText++;
        //        DOVirtual.Color(parentImage.color, new Color(1, 1, 1, 1f), 1.5f, (color) =>
        //        {
        //            parentImage.color = color;
        //        }).SetEase(Ease.Linear);
        //    },
        //    () => {
        //        _player.controller.OnSquarePress += SquarePressed;
        //        _player.controller.OnTrianglePress += TrianglePressed;
        //        _player.controller.OnSquareHold += SquareHolded;                
        //        _player.controller.OnTriangleHold += TriangleHolded;
        //    }, 0));


        if (currentState == Enums.TutorialState.FINISHED)
        {
            if (!pyramidTracker.activeSelf)
                pyramidTracker.SetActive(true);

            if (enemyDamageableCol != null && !enemyDamageableCol.enabled)
                enemyDamageableCol.enabled = true;

            if (enemyHealthUI != null && !enemyHealthUI.activeSelf)
                enemyHealthUI.SetActive(true);
        }

    }

    //UI MANAGEMENT COROUTINES

    IEnumerator IntroSpeach()
    {
        yield return new WaitForSeconds(1.5f);

        for (int i = 0; i < dialogues.Length; i++)
        {
            voice.Speak(dialogues[i], name);
            yield return new WaitUntil(() => voice.playing == false);
            yield return new WaitForSeconds(0.8f);
        }
    }

    IEnumerator ResetImages()
    {

        foreach (Image image in uiCombosImage)
        {
            DOVirtual.Color(image.color, Color.white, 0.80f, (col) =>
            {
                image.color = col;
            }).SetEase(Ease.InOutSine);

        }

        yield return new WaitForSeconds(0.8f);

        Color color = Color.white;
        color.a = 0;

        foreach (Image image in uiCombosImage)
        {
            DOVirtual.Color(image.color, color, 0.5f, (col) =>
            {
                image.color = col;
            }).SetEase(Ease.InOutSine);

        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator StartPhase(Enums.TutorialState newPhase, int imagesNeeded, System.Action onFinishCurrentAction, System.Action onStartNewAction, float delayOnEnter)
    {      
        onFinishCurrentAction();

        //if (newPhase != TutorialState.JUMP && (newPhase != TutorialState.COMBOS || (newPhase == TutorialState.COMBOS && currentComboLevel != 0)))
        //    AudioManager.Instance.PlayDelayFx(Enums.Effects.Positivo, 0.4f);

        yield return new WaitForSeconds(delayOnEnter);        

        yield return StartCoroutine(ResetImages());

        for (int i = 0; i < 4; i++)
        {
            uiCombosImage[i].sprite = matchKeyWithUI[(int)newPhase].value;
            uiCombosImage[i].enabled = false;
            uiCombosImage[i].gameObject.SetActive(false);
        }

        //Appear new images, enabled only necessary ones and transparent to 1

        for (int i = 0; i < 4; i++)
        {
            if (i <= imagesNeeded - 1)
            {
                if (newPhase != Enums.TutorialState.COMBOS)
                    uiCombosImage[i].sprite = matchKeyWithUI[(int)newPhase].value;
                //else
                //    uiCombosImage[i].sprite = matchKeyWithUI.Where(e => e.key == tutorialPhasesInputs[currentComboLevel].collection[i]).Select(e => e.value).First();

                if (!uiCombosImage[i].enabled)
                    uiCombosImage[i].enabled = true;

                if (!uiCombosImage[i].gameObject.activeSelf)
                    uiCombosImage[i].gameObject.SetActive(true);

                DOVirtual.Color(uiCombosImage[i].color, Color.white, 0.6f, (col) =>
                {
                    uiCombosImage[i].color = col;
                }).SetEase(Ease.InOutSine);

                yield return new WaitForSeconds(0.6f);
            }

        }

        ChangeTutorialPhase(newPhase);

        onStartNewAction();

    }

    IEnumerator RestartCombo(System.Action onPhaseRestarted)
    {
        AudioManager.Instance.PlayDelayFx(Effects.ErrorButton, 0);
        yield return new WaitForSeconds(1.3f);

        AudioManager.Instance.PlayDelayFx(Enums.Effects.Negativo, 0.5f);
        foreach (Image image in uiCombosImage)
        {
            DOVirtual.Color(image.color, Color.red, 0.80f, (col) =>
            {
                image.color = col;
            }).SetEase(Ease.InOutSine);

        }

        yield return new WaitForSeconds(0.8f);

        Color color = Color.white;
        color.a = 0;

        foreach (Image image in uiCombosImage)
        {
            DOVirtual.Color(image.color, color, 0.5f, (col) =>
            {
                image.color = col;
            }).SetEase(Ease.InOutSine);

        }

        yield return new WaitForSeconds(1f);

        foreach (Image image in uiCombosImage)
        {
            DOVirtual.Color(image.color, Color.white, 0.80f, (col) =>
            {
                image.color = col;
            }).SetEase(Ease.InOutSine);

        }

        

        onPhaseRestarted();       
    }

    IEnumerator HideTutorial(System.Action onTutorialHided)
    {
        yield return new WaitForSeconds(1);

        yield return StartCoroutine(ResetImages());

        Image parentImage = uiCombosImage[0].transform.parent.GetComponent<Image>();

        DOVirtual.Color(parentImage.color, new Color(1, 1, 1, 0), 1f, (color) =>
        {
            parentImage.color = color;
        }).SetEase(Ease.Linear);

        yield return new WaitForSeconds(1.1f);

        for (int i = 1; i < uiCombosImage.Length - 1; i++)
        {
            uiCombosImage[i].enabled = false;
            uiCombosImage[i].gameObject.SetActive(false);
        }
        uiCombosImage[uiCombosImage.Length - 1].sprite = emptySprite;
        uiCombosImage[uiCombosImage.Length - 1].color = Color.white;

        onTutorialHided();
    }

    //HANDLE MOVEMENT INPUT

    private void MoveLeftStick_performed(InputAction.CallbackContext context)
    {
        walking = true;
    }
    private void MoveLeftStick_canceled(InputAction.CallbackContext context)
    {
        walking = false;
    }
    private void Jump_performed(InputAction.CallbackContext context)
    {
        

        DOVirtual.Color(uiCombosImage[0].color, Color.green, 1f, (color) =>
        {
            uiCombosImage[0].color = color;
        }).SetEase(Ease.InOutSine);

        
        //StartCoroutine(StartPhase(Enums.TutorialState.DOUBLEJUMP, 2, () =>
        //{
        //    _playerActions.Player.Jump.performed -= Jump_performed;
        //}, () =>
        //{
        //    _playerActions.Player.Jump.performed += DoubleJump_performed;            
        //}, 1));
        

    }

    private void DoubleJump_performed(InputAction.CallbackContext context)
    {
        lastDoubleJumpTime = Time.time;
        jumpsDone += 1;
        int ind = jumpsDone;

        DOVirtual.Color(uiCombosImage[ind].color, Color.green, 1f, (color) =>
        {
            uiCombosImage[ind].color = color;
        }).SetEase(Ease.InOutSine);

        if (ind >= 1)
        {
            jumpsDone = -1;
            //StartCoroutine(StartPhase(Enums.TutorialState.SPRINT, 1, () =>
            //{
            //    _playerActions.Player.Jump.performed -= DoubleJump_performed;
            //}, () =>
            //{
            //    _playerActions.Player.R2Pressed.performed += SprintPerformed;
            //    _playerActions.Player.Movement.performed += MoveLeftStick_performed;
            //    _playerActions.Player.Movement.canceled += MoveLeftStick_canceled;
            //}, 1));
        }

    }
    

    private void RollPerformed(InputAction.CallbackContext context)
    {
        if (!walking)
            return;

        AudioManager.Instance.PlayDelayFx(Enums.Effects.Positivo, 0.4f);
        //rollsDone += 1;
        //int ind = rollsDone;

        DOVirtual.Color(uiCombosImage[0].color, Color.green, 1f, (color) =>
        {
            uiCombosImage[0].color = color;
        }).SetEase(Ease.InOutSine);

        //if (ind >= 1)
        //{
            _playerActions.Player.Dash.performed -= RollPerformed;
            _playerActions.Player.Movement.performed -= MoveLeftStick_performed;
            _playerActions.Player.Movement.canceled -= MoveLeftStick_canceled;


            StartCoroutine(HideTutorial(() => { canInteract = true; objectiveMarker.SetActive(true); uiCombosImage[0].sprite = emptySprite; }));
            inputsBuffer.Clear();
        //}
    }
    private void SprintPerformed(InputAction.CallbackContext context)
    {

        if (walking)
        {
            DOVirtual.Color(uiCombosImage[0].color, Color.green, 1f, (color) =>
            {
                uiCombosImage[0].color = color;
            }).SetEase(Ease.InOutSine);

            //StartCoroutine(StartPhase(Enums.TutorialState.ROLL, 1, () =>
            //{
            //    _playerActions.Player.R2Pressed.performed -= SprintPerformed;
            //}, () =>
            //{
            //    _playerActions.Player.Dash.performed += RollPerformed;
            //}, 1));
        }
    }

    //HANDLE COMBOS INPUT

    void SquarePressed() { 
        inputsBuffer.Add(Enums.TutorialInputs.Square);
        lastAttackTime = Time.time;
    }
    void SquareHolded()
    {
        inputsBuffer.Add(Enums.TutorialInputs.HoldSquare);
        lastAttackTime = Time.time;
    }
    void TrianglePressed()
    {
        inputsBuffer.Add(Enums.TutorialInputs.Triangle);
        lastAttackTime = Time.time;
    }
    void TriangleHolded()
    {
        inputsBuffer.Add(Enums.TutorialInputs.HoldTriangle);
        lastAttackTime = Time.time;
    }

    void StepDone()
    {
        int index = inputsBuffer.Count() - 1;
        DOVirtual.Color(uiCombosImage[index].color, Color.green, 1f, (color) =>
        {
            uiCombosImage[index].color = color;
        }).SetEase(Ease.InOutSine);
    }

    void ComboCompleted()
    {
        Debug.Log("Completed");
        inputsBuffer.Clear();
        currentComboLevel++;
        _isL2Performed = false;        


        if(currentComboLevel >= tutorialPhasesInputs.Count)
        {
            AudioManager.Instance.PlayDelayFx(Enums.Effects.Positivo, 0.4f);
            ChangeTutorialPhase(Enums.TutorialState.FINISHED);
            _player.controller.OnSquarePress -= SquarePressed;
            _player.controller.OnSquareHold -= SquareHolded;
            _player.controller.OnTrianglePress -= TrianglePressed;
            _player.controller.OnTriangleHold -= TriangleHolded;
            StartCoroutine(HideTutorial(() => { canInteract = true; objectiveMarker.SetActive(true); }));
        }            
        else
            StartCoroutine(StartPhase(Enums.TutorialState.COMBOS, tutorialPhasesInputs[currentComboLevel].collection.Count(), () =>
            {
                _player.controller.OnSquarePress -= SquarePressed;
                _player.controller.OnSquareHold -= SquareHolded;
                _player.controller.OnTrianglePress -= TrianglePressed;
                _player.controller.OnTriangleHold -= TriangleHolded;
            }, () =>
            {
                _player.controller.OnSquarePress += SquarePressed;
                _player.controller.OnSquareHold += SquareHolded;
                _player.controller.OnTrianglePress += TrianglePressed;
                _player.controller.OnTriangleHold += TriangleHolded;
            }, 1));

    }

    void ComboFailed(int extra = 0)
    {
        _player.controller.OnSquarePress -= SquarePressed;
        _player.controller.OnSquareHold -= SquareHolded;
        _player.controller.OnTrianglePress -= TrianglePressed;
        _player.controller.OnTriangleHold -= TriangleHolded;

        int ind = inputsBuffer.Count() - 1 + extra;
        
        DOVirtual.Color(uiCombosImage[ind].color, Color.red, 1f, (color) =>
        {
            uiCombosImage[ind].color = color;
        }).SetEase(Ease.InOutSine);

        inputsBuffer.Clear();

        StartCoroutine(RestartCombo(() =>
        {
            _player.controller.OnSquarePress += SquarePressed;
            _player.controller.OnSquareHold += SquareHolded;
            _player.controller.OnTrianglePress += TrianglePressed;
            _player.controller.OnTriangleHold += TriangleHolded;
        }));
    } 

    void ChangeTutorialPhase(Enums.TutorialState newState)
    {
        currentState = newState;
    }    

    public Enums.TutorialState TutorialState { get { return currentState; } }


}

