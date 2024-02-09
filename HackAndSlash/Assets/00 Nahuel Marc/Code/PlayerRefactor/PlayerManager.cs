using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerManager : MonoBehaviour
{
    public PlayerAnimations animations { get; protected set; }
    public PlayerStadistics stats { get; protected set; }
    public PlayerMovement movement { get; protected set; }
    public Rigidbody rb { get; protected set; }
    public PlayerHUDSystem hud { get; protected set; }
    public PlayerHealthSystem healthSystem { get; protected set; }
    public PlayerInputsSystem inputs { get; private set; }


    public bool isDead = false;
    public bool isStun = false;
    public bool isInteracting;
    [SerializeField] private Enums.CharacterState _currentCharacterState = Enums.CharacterState.Idle;

    public Enums.CharacterState CurrentCharacterState
    {
        get => _currentCharacterState;
        set => _currentCharacterState = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animations = transform.GetChild(0).GetComponent<PlayerAnimations>();
        movement = GetComponent<PlayerMovement>();
        hud = GetComponent<PlayerHUDSystem>();
        inputs = GetComponent<PlayerInputsSystem>();        
    }

    void Update()
    {
        if(CurrentCharacterState == Enums.CharacterState.Idle)
        {
            
        }
        else if(CurrentCharacterState == Enums.CharacterState.Moving)
        {

        }
    }

    private void FixedUpdate()
    {
        if(!isStun)
        {
            movement.HandleAllMovement();
        }
    }

    private void LateUpdate()
    {
        //cameraScript.UpdateCameraPostion(transfrom)
        isInteracting = animations.GetAnimator.GetBool("isInteracting");
    }

    public void ChangeCharacterState(Enums.CharacterState newState)
    {
        if(CurrentCharacterState == newState) { return; }

        CurrentCharacterState = newState;
        switch (CurrentCharacterState)
        {
            case Enums.CharacterState.Idle:
                break;
            case Enums.CharacterState.Moving:
                break;
            case Enums.CharacterState.Dashing:
                break;
            case Enums.CharacterState.Attacking:
                break;
            default:
                break;
        }
    }
}
