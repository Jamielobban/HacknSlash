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
    [SerializeField] bool intro, tutorialDone;
    
    int currentAction = 0;
    readonly string name = "Cyborg Sergeant";
    private PlayerInputActionsRefactor _playerActions;

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
        combosToPractice = FindObjectOfType<PlayerMovement>().GetComponentsInChildren<Combo>().Select(c => c.sequence).ToList();
        _playerActions.Player.Square.performed += SquarePerformed;
        _playerActions.Player.Triangle.performed += TrianglePerformed;
    }

    private void Update()
    {
        if(!intro && !tutorialDone)
        {
            if (inputsBuffer.Count() <= 0)
                return;

            if (inputsBuffer.Last() != combosToPractice[currentAction][inputsBuffer.Count()-1])
                StepFailed();
            else {
                StepCompleted();
                if (inputsBuffer.Count() == combosToPractice[currentAction].Count())
                    ComboCompleted();
            }               

        }
    }


    public void Interact()
    {
       // if (!canInteract) return;

        voice.Speak(dialogues[currentAction], name);
    }

    void TrianglePerformed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)        
            inputsBuffer.Add(Enums.InputsAttack.HoldSquare);        
        else        
            inputsBuffer.Add(Enums.InputsAttack.Square);        
    }

    void SquarePerformed(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
            inputsBuffer.Add(Enums.InputsAttack.HoldTriangle);
        else
            inputsBuffer.Add(Enums.InputsAttack.Triangle);
    }

    void StepCompleted()
    {
    }

    void ComboCompleted()
    {
        currentAction++;
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

