using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class BlackCyborgSoldier : Interactive, IInteractable
{
    [TextArea]
    [SerializeField] string[] dialogues;
    [SerializeField] List<Combo>combosToPractice = new List<Combo>();
    [SerializeField] List<Sprite> uiCombos = new List<Sprite>();
    [SerializeField] SimpleRTVoiceExample voice;
    [SerializeField] bool intro, hasSprinted = false;
    [SerializeField] Image[] uiCombosImage = new Image[4];
    //[SerializeField] List<List<Enums.InputsAttack>> combosToPractice = new List<List<Enums.InputsAttack>>();


    readonly string name = "Cyborg Sergeant";
    int currentText = 0, currentComboLevel = 0, jumpsDone = 0, rollsDone = 0;
    bool _isL2Performed;
    List<Enums.InputsAttack> inputsBuffer = new List<Enums.InputsAttack>();
    Dictionary<Enums.InputsAttack, Sprite> matchKeyWithUI = new Dictionary<Enums.InputsAttack, Sprite>();
    Enums.TutorialState currentState = Enums.TutorialState.INACTIVE;
    PlayerInputActionsRefactor _playerActions;

    
    private void Awake()
    {
        _playerActions = new PlayerInputActionsRefactor();
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

        
        if (currentState == Enums.TutorialState.JUMPS && jumpsDone >= 3)
        {
            ChangeTutorialPhase(Enums.TutorialState.SPRINT);
            for (int i = 1; i < 3; i++)
            {
                uiCombosImage[i].enabled = false;
            }
            uiCombosImage[0].sprite = uiCombos[1];
            _playerActions.Player.Dash.performed += SprintPerformed;
            _playerActions.Player.Dash.performed -= Jump_performed;
        }
        else if (currentState == Enums.TutorialState.SPRINT && hasSprinted)
        {
            ChangeTutorialPhase(Enums.TutorialState.ROLLS);
            for (int i = 0; i < 3; i++)
            {
                uiCombosImage[i].enabled = true;
                uiCombosImage[0].sprite = uiCombos[2];
            }
            _playerActions.Player.Dash.performed += RollPerformed;
            _playerActions.Player.Dash.performed -= SprintPerformed;
        }
        else if (currentState == Enums.TutorialState.ROLLS && rollsDone >= 3)
        {
            ChangeTutorialPhase(Enums.TutorialState.COMBOS);
            _playerActions.Player.Square.performed += SquarePerformed;
            _playerActions.Player.Triangle.performed += TrianglePerformed;
            _playerActions.Player.L2.performed += L2_performed;
            _playerActions.Player.L2.canceled += L2_canceled;
            _playerActions.Player.Dash.performed -= RollPerformed;
        }
        else if(currentState == Enums.TutorialState.COMBOS && currentComboLevel > combosToPractice.Count)
        {
            ChangeTutorialPhase(Enums.TutorialState.FINISHED);
            _playerActions.Player.Square.performed -= SquarePerformed;
            _playerActions.Player.Triangle.performed -= TrianglePerformed;
            _playerActions.Player.L2.performed -= L2_performed;
            _playerActions.Player.L2.canceled -= L2_canceled;
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
            _playerActions.Player.Jump.performed += Jump_performed;

            ChangeTutorialPhase(Enums.TutorialState.JUMPS);

            uiCombosImage[0].transform.parent.gameObject.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                uiCombosImage[i].enabled = true;
                uiCombosImage[i].sprite = uiCombos[0];
            }
        }
        else
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
        jumpsDone++;
    }
    private void RollPerformed(InputAction.CallbackContext context)
    {
        rollsDone++;
    }

    private void SprintPerformed(InputAction.CallbackContext context)
    {
        hasSprinted = true;
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

}

