using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System.Threading.Tasks;

public class BlackCyborgSoldier : Interactive, IInteractable
{
    [TextArea]
    [SerializeField] string[] dialogues;
    [SerializeField] List<Combo>combosToPractice = new List<Combo>();
    [SerializeField] List<Sprite> uiCombos = new List<Sprite>();
    [SerializeField] SimpleRTVoiceExample voice;
    [SerializeField] bool intro, hasSprinted = false, walking = false;
    [SerializeField] Image[] uiCombosImage = new Image[4];
    //[SerializeField] List<List<Enums.InputsAttack>> combosToPractice = new List<List<Enums.InputsAttack>>();


    readonly string name = "Cyborg Sergeant";
    int currentText = 0, currentComboLevel = 0, jumpsDone = -1, rollsDone = -1;
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

        matchKeyWithUI.Add(Enums.InputsAttack.Square, uiCombos[3]);
        matchKeyWithUI.Add(Enums.InputsAttack.HoldSquare, uiCombos[4]);
        matchKeyWithUI.Add(Enums.InputsAttack.Triangle, uiCombos[5]);
        matchKeyWithUI.Add(Enums.InputsAttack.HoldTriangle, uiCombos[6]);
        matchKeyWithUI.Add(Enums.InputsAttack.L2Square, uiCombos[7]);
        matchKeyWithUI.Add(Enums.InputsAttack.L2Triangle, uiCombos[8]);
        //combosToPractice.AddRange(FindObjectOfType<PlayerMovement>().GetComponentsInChildren<Combo>().Select(c => c.sequence).ToList());
       
    }

    private void Update()
    {
        if (intro || currentState == Enums.TutorialState.INACTIVE || currentState == Enums.TutorialState.FINISHED)
            return;

        
        
        
        if(currentState == Enums.TutorialState.COMBOS && currentComboLevel > combosToPractice.Count)
        {
            ChangeTutorialPhase(Enums.TutorialState.FINISHED);
            _playerActions.Player.Square.performed -= SquarePerformed;
            _playerActions.Player.Triangle.performed -= TrianglePerformed;
            _playerActions.Player.L2.performed -= L2_performed;
            _playerActions.Player.L2.canceled -= L2_canceled;
            _playerActions.Player.Disable();
        }
        else if (currentState == Enums.TutorialState.COMBOS)
        {
            if (inputsBuffer.Count() <= 0)
                return;

            if (inputsBuffer.Last() != combosToPractice[currentComboLevel].sequence[inputsBuffer.Count() - 1])
                StepFailed();
            else
            {
                StepCompleted();
                if (inputsBuffer.Count() == combosToPractice[currentComboLevel].sequence.Count())
                    ComboCompleted();
            }

        }
    }


    public void Interact()
    {
       // if (!canInteract) return;

        voice.Speak(dialogues[currentText], name);

        if (!intro && currentState == Enums.TutorialState.INACTIVE)
        {
            Image parentImage = uiCombosImage[0].transform.parent.GetComponent<Image>();
            StartPhase(Enums.TutorialState.JUMPS, 3, () => {
                DOVirtual.Color(parentImage.color, new Color(1,1,1,0.38f), 0.5f, (color) =>
                {
                    parentImage.color = color;
                }).SetEase(Ease.Linear);
                uiCombosImage[0].transform.parent.GetComponent<Image>(); 
            }, () => { _playerActions.Player.Jump.performed += Jump_performed; }, 0.15f);            
        }
        else if(!intro)
            currentText++;
    }

    void TrianglePerformed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)        
            inputsBuffer.Add(Enums.InputsAttack.HoldTriangle);        
        else
        {
            if(_isL2Performed)
                inputsBuffer.Add(Enums.InputsAttack.L2Triangle);
            else
                inputsBuffer.Add(Enums.InputsAttack.Triangle);
        }
    }

    void SquarePerformed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
            inputsBuffer.Add(Enums.InputsAttack.HoldSquare);
        else
        {
            if (_isL2Performed)
                inputsBuffer.Add(Enums.InputsAttack.L2Square);
            else
                inputsBuffer.Add(Enums.InputsAttack.Square);
        }
    }

    private void L2_performed(InputAction.CallbackContext context)
    {
        _isL2Performed = true;
    }
    private void L2_canceled(InputAction.CallbackContext context)
    {
        _isL2Performed = false;
    }

    private void Jump_performed(InputAction.CallbackContext context)
    {
        jumpsDone += 1;
        DOVirtual.Color(uiCombosImage[jumpsDone].color, Color.green, 1f, (color) =>
        {
            uiCombosImage[jumpsDone].color = color;
        }).SetEase(Ease.InOutSine);

        if (jumpsDone >= 2)
        {
            StartPhase(Enums.TutorialState.SPRINT, 1, () =>
            {
                _playerActions.Player.Jump.performed -= Jump_performed;
            }, () =>
            {
                _playerActions.Player.R2.performed += SprintPerformed;
                _playerActions.Player.Movement.performed += MoveLeftStick_performed;
                _playerActions.Player.Movement.canceled += MoveLeftStick_canceled;
            }, 1);
        }

    }
    private void RollPerformed(InputAction.CallbackContext context)
    {
        rollsDone += 1;

        DOVirtual.Color(uiCombosImage[rollsDone].color, Color.green, 1f, (color) =>
        {
            uiCombosImage[rollsDone].color = color;
        }).SetEase(Ease.InOutSine);

        if (rollsDone >= 2)
        {
            StartPhase(Enums.TutorialState.COMBOS, 1, () =>
            {
                _playerActions.Player.Dash.performed -= RollPerformed;
            }, () =>
            {
                _playerActions.Player.Square.performed += SquarePerformed;
                _playerActions.Player.Triangle.performed += TrianglePerformed;
                _playerActions.Player.L2.performed += L2_performed;
                _playerActions.Player.L2.canceled += L2_canceled;
                _playerActions.Player.Dash.performed -= RollPerformed;
            }, 1);           
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

            StartPhase(Enums.TutorialState.ROLLS, 3, () =>
            {
                _playerActions.Player.R2.performed -= SprintPerformed;
                _playerActions.Player.Movement.performed -= MoveLeftStick_performed;
                _playerActions.Player.Movement.canceled -= MoveLeftStick_canceled;
            }, () =>
            {
                _playerActions.Player.Dash.performed += RollPerformed;
            }, 1);
        }
    }

    private void MoveLeftStick_performed(InputAction.CallbackContext context)
    {
        walking = true;
    }
    private void MoveLeftStick_canceled(InputAction.CallbackContext context)
    {
        walking = false;
    }


    void ChangeTutorialPhase(Enums.TutorialState newState)
    {
        currentState = newState;
    }

    void StepCompleted()
    {

    }

    void ComboCompleted()
    {
        inputsBuffer.Clear();
        currentComboLevel++;
    }

    void StepFailed()
    {
        inputsBuffer.Clear();
    }

    IEnumerator IntroSpeach()
    {
        yield return new WaitForSeconds(1.5f);
        voice.Speak(dialogues[0], name);
    }

    //async void ResetImages()
    //{
        

    //    await Task.Delay(400);
    //}

    async void StartPhase(Enums.TutorialState newPhase, int imagesNeeded, System.Action onFinishCurrentAction, System.Action onStartNewAction, float delayOnEnter)
    {

        onFinishCurrentAction();

        await Task.Delay((int)delayOnEnter * 1000);
        //Disappear old images, color to white and transparent

        for (int i = 0; i < 4; i++)
        {
            DOVirtual.Color(uiCombosImage[i].color, new Color(1, 1, 1, 0), 0.40f, (col) =>
            {
                uiCombosImage[i].color = col;
            }).SetEase(Ease.InOutSine);
        }

        await Task.Delay((int)3000);

        for (int i = 0; i < 4; i++)
        {
            uiCombosImage[i].sprite = uiCombos[(int)newPhase];
        }

        //Appear new images, enabled only necessary ones and transparent to 1

        for (int i = 3; i >= 0; i--)
        {
            if (i > imagesNeeded - 1)
            {
                if (uiCombosImage[i].enabled)
                    uiCombosImage[i].enabled = false;
            }
            else
            {
                uiCombosImage[i].sprite = uiCombos[(int)newPhase];
                if (!uiCombosImage[i].enabled)
                    uiCombosImage[i].enabled = true;

                DOVirtual.Color(uiCombosImage[i].color, Color.white, 0.6f, (col) =>
                {
                    uiCombosImage[i].color = col;
                }).SetEase(Ease.InOutSine);

                await Task.Delay((int)600);
            }

        }

        ChangeTutorialPhase(newPhase);

        onStartNewAction();

    }

    //IEnumerator StartPhase(Enums.TutorialState newPhase, int imagesNeeded, System.Action onFinishCurrentAction, System.Action onStartNewAction, float delayOnEnter)
    //{

    //    onFinishCurrentAction();

    //    yield return new WaitForSeconds(delayOnEnter);
    //    //Disappear old images, color to white and transparent

    //    Task.Run(() => ResetImages());         

    //    for (int i = 0; i < 4; i++)
    //    {
    //        uiCombosImage[i].sprite = uiCombos[(int)newPhase];
    //    }

    //    //Appear new images, enabled only necessary ones and transparent to 1

    //    for (int i = 3; i >= 0; i--)
    //    {
    //        if(i > imagesNeeded - 1)
    //        {
    //            if(uiCombosImage[i].enabled)
    //                uiCombosImage[i].enabled = false;
    //        }
    //        else
    //        {
    //            uiCombosImage[i].sprite = uiCombos[(int)newPhase];               
    //            if (!uiCombosImage[i].enabled)
    //                uiCombosImage[i].enabled = true;

    //            DOVirtual.Color(uiCombosImage[i].color, Color.white, 0.6f, (col) =>
    //            {
    //                uiCombosImage[i].color = col;
    //            }).SetEase(Ease.InOutSine);

    //            yield return new WaitForSeconds(0.6f);
    //        }

    //    }

    //    ChangeTutorialPhase(newPhase);

    //    onStartNewAction();

    //}

}

//SetEase((float time, float duration, float overshootOrAmplitude, float period) =>
//{
//    // Ajustar la fuerza de la función de ease
//    float linearEase = time / duration;
//    float easedValue = Mathf.Sin(linearEase * Mathf.PI * 0.5f); // Ease.InOutSine
//    float poweredValue = Mathf.Pow(easedValue, 10f); // Aplicar potencia para exagerar el efecto
//    return poweredValue;
//});