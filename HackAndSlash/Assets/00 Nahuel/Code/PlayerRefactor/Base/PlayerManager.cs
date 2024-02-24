using DamageNumbersPro.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Player Scripts References
    public PlayerAnimations animations { get; protected set; }
    public PlayerStadistics stats { get; protected set; }
    public PlayerMovement movement { get; protected set; }
    public PlayerHUDSystem hud { get; protected set; }
    public PlayerHealthSystem healthSystem { get; protected set; }
    public PlayerInputsSystem inputs { get; private set; }
    public PlayerInventory inventory { get; private set; }

    public PlayerCollision collision;
    public ComboController comboController { get; private set; }

    #endregion

    public Rigidbody rb { get; protected set; }
    public GameObject playerHandRight;

    private Camera _mainCamera;
    public Camera MainCamera { get => _mainCamera; }

    [SerializeField] private Enums.CharacterState _currentCharacterState = Enums.CharacterState.Idle;
    public Enums.CharacterState CurrentCharacterState
    {
        get => _currentCharacterState;
        set => _currentCharacterState = value;
    }

    public bool isDead = false;
    public bool isStun = false;
    public bool isInvulnerable = false;
    public bool isInAir = false;
    public bool isInteracting;
    public bool isAirAttacking = false;


    private void Awake()
    {
        _mainCamera = Camera.main;
        inventory = GetComponent<PlayerInventory>();
        comboController = GetComponent<ComboController>();
        rb = GetComponent<Rigidbody>();
        animations = GetComponent<PlayerAnimations>();
        movement = GetComponent<PlayerMovement>();
        hud = GetComponent<PlayerHUDSystem>();
        inputs = GetComponent<PlayerInputsSystem>();
        stats = GetComponent<PlayerStadistics>();
    }

    void Update()
    {
        if(CurrentCharacterState == Enums.CharacterState.Moving)
        {
            animations.HandleMovingAnimations();
        }
    }

    private void FixedUpdate()
    {
        if (!isStun)
        {
            movement.HandleAllMovement();
        }
    }

    private void LateUpdate()
    {
        isInteracting = animations.Animator.GetBool("isInteracting");
        isAirAttacking = animations.Animator.GetBool("isAirAttacking");
        movement.isJumping = animations.Animator.GetBool("isJumping");
        animations.Animator.SetBool("isGrounded", movement.isGrounded);
    }

    public void ChangeCharacterState(Enums.CharacterState newState)
    {
        if(CurrentCharacterState == newState) { return; }

        CurrentCharacterState = newState;
        switch (CurrentCharacterState)
        {
            case Enums.CharacterState.Idle:
                animations.OnIdleAnimation();
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
