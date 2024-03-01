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

    private void Awake()
    {
        _player = transform.parent.parent.GetComponent<PlayerManager>();
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distToGround, groundMask);
        
        if (CanApplyDrag())
            _player.rb.drag = groundDrag;
        else
            _player.rb.drag = 0;
    }

    private bool CanApplyDrag() => _player.movement.moveState == Enums.PlayerMovementState.Walking || _player.movement.moveState == Enums.PlayerMovementState.Sprinting;
}
