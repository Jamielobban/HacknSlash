using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class BlackCyborgSoldier : Interactive, IInteractable
{
    [TextArea]
    [SerializeField] string[] dialogues;
    [SerializeField] List<List<Enums.InputsAttack>> combosToPractice = new List<List<Enums.InputsAttack>>();
    List<Enums.InputsAttack> inputsBuffer = new List<Enums.InputsAttack>(); 
    [SerializeField] SimpleRTVoiceExample voice;
    [SerializeField] bool intro, tutorialDone, tutorialStarted, jumpDone = false, rollDone = false;
    [SerializeField] List<Sprite> psUI = new List<Sprite>();
    [SerializeField] 
    int currentText = 0, currentTutorialLevel = 0, currentComboLevel = 0, jumpsDone = 0, rollsDone = 0;
    
    readonly string name = "Cyborg Sergeant";
    private PlayerInputActionsRefactor _playerActions;
    private bool _isL2Performed;

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

        combosToPractice.AddRange(FindObjectOfType<PlayerMovement>().GetComponentsInChildren<Combo>().Select(c => c.sequence).ToList());
       
    }

    private void Update()
    {
        if(!intro && !tutorialDone)
        {
            if (inputsBuffer.Count() <= 0)
                return;

            if (inputsBuffer.Last() != combosToPractice[currentText-1][inputsBuffer.Count()-1])
                StepFailed();
            else {
                StepCompleted();
                if (inputsBuffer.Count() == combosToPractice[currentText-1].Count())
                    ComboCompleted();
            }               

        }
    }


    public void Interact()
    {
       // if (!canInteract) return;

        voice.Speak(dialogues[currentText], name);

        if(!intro && !tutorialDone && !tutorialStarted)
        {
            _playerActions.Player.Square.performed += SquarePerformed;
            _playerActions.Player.Triangle.performed += TrianglePerformed;
            _playerActions.Player.L2.performed += L2_performed;
            _playerActions.Player.L2.canceled += L2_canceled;
            _playerActions.Player.Jump.performed += Jump_performed;
            _playerActions.Player.Dash.performed += Dash_performed;

            tutorialStarted = true;
        }
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
        jumpDone = true;
    }
    private void Dash_performed(InputAction.CallbackContext context)
    {
        rollDone = true;
    }

    void StepCompleted()
    {

    }

    void ComboCompleted()
    {
        inputsBuffer.Clear();
        currentText++;
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

