using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    private PlayerManager _player;

    public bool isGrounded;
    public float distToGround;
    public float groundDrag = 10f;
    public LayerMask groundMask;
    public RaycastHit hitGround;
    private void Awake()
    {
        _player = transform.parent.parent.GetComponent<PlayerManager>();
    }

    private void Update()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out hitGround, distToGround, groundMask))
        {
            if (!_player.groundCheck.isGrounded)
            {
                _player.animations.Animator.SetBool("isGrounded", true);
            }

            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (CanApplyDrag())
            _player.rb.drag = groundDrag;
        else
            _player.rb.drag = 2;
    }

    public RaycastHit GetHit() => hitGround;

    private bool CanApplyDrag() => _player.movement.moveState == Enums.PlayerMovementState.Walking || _player.movement.moveState == Enums.PlayerMovementState.Sprinting;
}
