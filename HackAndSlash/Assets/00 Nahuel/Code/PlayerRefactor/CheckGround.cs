using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public bool isGrounded;
    public float distToGround;
    public LayerMask groundMask;
    public float groundDrag = 10f;
    private PlayerManager _player;

    private void Awake()
    {
        _player = transform.parent.parent.GetComponent<PlayerManager>();
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, distToGround, groundMask);
        if (isGrounded)
            _player.rb.drag = groundDrag;
        else if(!isGrounded && !_player.movement.isDashing)
            _player.rb.drag = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
       // isGrounded = true;
    }
    private void OnTriggerStay(Collider other)
    {
       // isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
       // isGrounded = false;
    }
}
