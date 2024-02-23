using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class BlackCyborgSoldier : Interactive, IInteractable
{
    //DIALOGUE 
    [TextArea]
    [SerializeField] string[] dialogues;

    [SerializeField] List<Combo>combosToPractice = new List<Combo>();
    [SerializeField] List<Sprite> uiTutorialButtons = new List<Sprite>();
    [SerializeField] SimpleRTVoiceExample voice;
    [SerializeField] bool intro, hasSprinted = false, walking = false;
    [SerializeField] Image[] uiCombosImage = new Image[4];
    [SerializeField] Sprite emptySprite;
    [SerializeField] GameObject objectiveMarker;

    readonly string name = "Cyborg Sergeant";
    int currentText = 0, currentComboLevel = 0, jumpsDone = -1, rollsDone = -1, lastBufferSize = 0;
    float lastAttackTime = 0;
    bool _isL2Performed;
    List<Enums.InputsAttack> inputsBuffer = new List<Enums.InputsAttack>();
    Dictionary<Enums.InputsAttack, Sprite> matchKeyWithUI = new Dictionary<Enums.InputsAttack, Sprite>();
    Enums.TutorialState currentState = Enums.TutorialState.INACTIVE;
    PlayerInputActionsRefactor _playerActions;

    
    private void Awake()
    {
        _playerActions = new PlayerInputActionsRefactor();
        _playerActions.Player.Enable();
    }

    private void Start()
    {
        if (intro)
        {
            StartCoroutine(IntroSpeach());
            return;
        }

        

        matchKeyWithUI.Add(Enums.InputsAttack.Square, uiTutorialButtons[3]);
        matchKeyWithUI.Add(Enums.InputsAttack.HoldSquare, uiTutorialButtons[4]);
        matchKeyWithUI.Add(Enums.InputsAttack.Triangle, uiTutorialButtons[5]);
        matchKeyWithUI.Add(Enums.InputsAttack.HoldTriangle, uiTutorialButtons[6]);
        matchKeyWithUI.Add(Enums.InputsAttack.L2Square, uiTutorialButtons[7]);
        matchKeyWithUI.Add(Enums.InputsAttack.L2Triangle, uiTutorialButtons[8]);        
       
    }

    private void Update()
    {
        if (canInteract && currentState != Enums.TutorialState.FINISHED)
            objectiveMarker.SetActive(true);

        if (intro || currentState == Enums.TutorialState.INACTIVE || currentState == Enums.TutorialState.FINISHED)
            return;

        if (currentState == Enums.TutorialState.COMBOS)
        {
            if (lastAttackTime == 0 || inputsBuffer.Count() > combosToPractice[currentComboLevel].sequence.Count() || inputsBuffer.Count == 0)
                return;

            if(Time.time - lastAttackTime >= combosToPractice[currentComboLevel].attack[inputsBuffer.Count() - 1].data.animationLength + 0.25f)
                ComboFailed(1);
            else
            {
                if (inputsBuffer.Count() <= 0 || lastBufferSize == inputsBuffer.Count() || lastAttackTime == 0)
                    return;

                if (inputsBuffer.Last() != combosToPractice[currentComboLevel].sequence[inputsBuffer.Count() - 1])
                {
                    ComboFailed();
                }
                else
                {
                    StepDone();
                    if (inputsBuffer.Count() == combosToPractice[currentComboLevel].sequence.Count())
                        ComboCompleted();
                }
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

        if(currentState == Enums.TutorialState.INACTIVE)
            StartCoroutine(StartPhase(Enums.TutorialState.JUMPS, 3, 
            () => {
                canInteract = false;
                currentText++;
                DOVirtual.Color(parentImage.color, new Color(1, 1, 1, 0.38f), 1.5f, (color) =>
                {
                    parentImage.color = color;
                }).SetEase(Ease.Linear);
            }, 
            () => { 
                _playerActions.Player.Jump.performed += Jump_performed; 
            },0));            
        else if(currentState == Enums.TutorialState.ROLLS)
            StartCoroutine(StartPhase(Enums.TutorialState.COMBOS, combosToPractice[currentComboLevel].sequence.Count(),
            () => {
                canInteract = false;
                currentText++;
                DOVirtual.Color(parentImage.color, new Color(1, 1, 1, 0.38f), 1.5f, (color) =>
                {
                    parentImage.color = color;
                }).SetEase(Ease.Linear);
            },
            () => {
                _playerActions.Player.Square.performed += SquarePerformed;
                _playerActions.Player.Triangle.performed += TrianglePerformed;
                _playerActions.Player.L2.performed += L2_performed;
                _playerActions.Player.L2.canceled += L2_canceled;
            }, 0));


    }

    //UI MANAGEMENT COROUTINES

    IEnumerator IntroSpeach()
    {
        yield return new WaitForSeconds(1.5f);
        voice.Speak(dialogues[0], name);
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

        yield return new WaitForSeconds(delayOnEnter);

        yield return StartCoroutine(ResetImages());

        for (int i = 0; i < 4; i++)
        {
            uiCombosImage[i].sprite = uiTutorialButtons[(int)newPhase];
            uiCombosImage[i].enabled = false;
            uiCombosImage[i].gameObject.SetActive(false);
        }

        //Appear new images, enabled only necessary ones and transparent to 1

        for (int i = 3; i >= 0; i--)
        {
            if (i <= imagesNeeded - 1)
            {
                if (newPhase != Enums.TutorialState.COMBOS)
                    uiCombosImage[i].sprite = uiTutorialButtons[(int)newPhase];
                else
                    uiCombosImage[i].sprite = matchKeyWithUI[combosToPractice[currentComboLevel].sequence[i]];

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

    IEnumerator RestartCombo()
    {
        yield return new WaitForSeconds(1.3f);

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

        yield return new WaitForSeconds(1f);

        _playerActions.Player.Square.performed += SquarePerformed;
        _playerActions.Player.Triangle.performed += TrianglePerformed;
        _playerActions.Player.L2.performed += L2_performed;
        _playerActions.Player.L2.canceled += L2_canceled;
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

        for(int i = 0; i < uiCombosImage.Length - 1; i++)
        {
            uiCombosImage[i].enabled = false;
            uiCombosImage[i].gameObject.SetActive(false);
        }
        uiCombosImage[uiCombosImage.Length-1].sprite = emptySprite;
        uiCombosImage[uiCombosImage.Length-1].color = Color.white;

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
        jumpsDone += 1;
        int ind = jumpsDone;

        DOVirtual.Color(uiCombosImage[ind].color, Color.green, 1f, (color) =>
        {
            uiCombosImage[ind].color = color;
        }).SetEase(Ease.InOutSine);

        if (ind >= 2)
        {
            StartCoroutine(StartPhase(Enums.TutorialState.SPRINT, 1, () =>
            {
                _playerActions.Player.Jump.performed -= Jump_performed;
            }, () =>
            {
                _playerActions.Player.R2.performed += SprintPerformed;
                _playerActions.Player.Movement.performed += MoveLeftStick_performed;
                _playerActions.Player.Movement.canceled += MoveLeftStick_canceled;
            }, 1));
        }

    }
    private void RollPerformed(InputAction.CallbackContext context)
    {
        if (!walking)
            return;

        rollsDone += 1;
        int ind = rollsDone;

        DOVirtual.Color(uiCombosImage[ind].color, Color.green, 1f, (color) =>
        {
            uiCombosImage[ind].color = color;
        }).SetEase(Ease.InOutSine);

        if (ind >= 2)
        {
            _playerActions.Player.Dash.performed -= RollPerformed;
            _playerActions.Player.Movement.performed -= MoveLeftStick_performed;
            _playerActions.Player.Movement.canceled -= MoveLeftStick_canceled;


            StartCoroutine(HideTutorial(() => { canInteract = true; objectiveMarker.SetActive(true); }));
            inputsBuffer.Clear();
        }        
    }
    private void SprintPerformed(InputAction.CallbackContext context)
    {

        if (walking)
        {
            DOVirtual.Color(uiCombosImage[0].color, Color.green, 1f, (color) =>
            {
                uiCombosImage[0].color = color;
            }).SetEase(Ease.InOutSine);

            StartCoroutine(StartPhase(Enums.TutorialState.ROLLS, 3, () =>
            {
                _playerActions.Player.R2.performed -= SprintPerformed;               
            }, () =>
            {
                _playerActions.Player.Dash.performed += RollPerformed;
            }, 1));
        }
    }

    //HANDLE COMBOS INPUT

    void TrianglePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Triangle");
        if (context.interaction is HoldInteraction)        
            inputsBuffer.Add(Enums.InputsAttack.HoldTriangle);        
        else
        {
            if(_isL2Performed)
                inputsBuffer.Add(Enums.InputsAttack.L2Triangle);
            else
                inputsBuffer.Add(Enums.InputsAttack.Triangle);
        }
        lastAttackTime = Time.time;
    }
    void SquarePerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Square");
        if (context.interaction is HoldInteraction)
            inputsBuffer.Add(Enums.InputsAttack.HoldSquare);
        else
        {
            if (_isL2Performed)
                inputsBuffer.Add(Enums.InputsAttack.L2Square);
            else
                inputsBuffer.Add(Enums.InputsAttack.Square);
        }
        lastAttackTime = Time.time;
    }
    private void L2_performed(InputAction.CallbackContext context)
    {
        _isL2Performed = true;
    }
    private void L2_canceled(InputAction.CallbackContext context)
    {
        _isL2Performed = false;
    }

    //EXTRA

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
        
        if(currentComboLevel >= combosToPractice.Count)
        {
            ChangeTutorialPhase(Enums.TutorialState.FINISHED);
            _playerActions.Player.Square.performed -= SquarePerformed;
            _playerActions.Player.Triangle.performed -= TrianglePerformed;
            _playerActions.Player.L2.performed -= L2_performed;
            _playerActions.Player.L2.canceled -= L2_canceled;
            StartCoroutine(HideTutorial(() => { canInteract = true; objectiveMarker.SetActive(true); }));
        }            
        else
            StartCoroutine(StartPhase(Enums.TutorialState.COMBOS, combosToPractice[currentComboLevel].sequence.Count(), () =>
            {
                _playerActions.Player.Square.performed -= SquarePerformed;
                _playerActions.Player.Triangle.performed -= TrianglePerformed;
                _playerActions.Player.L2.performed -= L2_performed;
                _playerActions.Player.L2.canceled -= L2_canceled;
            }, () =>
            {
                _playerActions.Player.Square.performed += SquarePerformed;
                _playerActions.Player.Triangle.performed += TrianglePerformed;
                _playerActions.Player.L2.performed += L2_performed;
                _playerActions.Player.L2.canceled += L2_canceled;
            }, 1));



    }

    void ComboFailed(int extra = 0)
    {
        _playerActions.Player.Square.performed -= SquarePerformed;
        _playerActions.Player.Triangle.performed -= TrianglePerformed;
        _playerActions.Player.L2.performed -= L2_performed;
        _playerActions.Player.L2.canceled -= L2_canceled;

        int ind = inputsBuffer.Count() - 1 + extra;
        
        DOVirtual.Color(uiCombosImage[ind].color, Color.red, 1f, (color) =>
        {
            uiCombosImage[ind].color = color;
        }).SetEase(Ease.InOutSine);

        inputsBuffer.Clear();

        StartCoroutine(RestartCombo());
    } 

    void ChangeTutorialPhase(Enums.TutorialState newState)
    {
        currentState = newState;
    }

    public Enums.TutorialState TutorialState { get { return currentState; } }
}

