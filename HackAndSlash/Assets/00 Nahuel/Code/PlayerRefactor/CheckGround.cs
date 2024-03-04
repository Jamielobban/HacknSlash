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
    float dist = 5f;
    private void Update()
    {

        if (Physics.Raycast(transform.position, Vector3.down, out hitGround, 50f, groundMask))
        {
            //Debug.Log(CalculateMaxDist());
            Debug.Log(_player.rb.velocity);
            if ((hitGround.distance < CalculateMaxDist() || hitGround.distance < 0.5f))
            {
                if (!isGrounded)
                {
                    if (!_player.animations.Animator.GetCurrentAnimatorStateInfo(0).IsName("land"))
                    {
                        _player.animations.Animator.CrossFadeInFixedTime("land", 0.2f);
                        _player.rb.velocity = new Vector3(_player.rb.velocity.x, 0, _player.rb.velocity.z);
                    }

                    if (!_player.movement.isJumping)
                    {
                        isGrounded = true;
                    }
                }
            }
            else
            {
                isGrounded = false;
            }
        }



        if (CanApplyDrag())
            _player.rb.drag = groundDrag;
        else
            _player.rb.drag = 5;
    }
    public float minDistanceRay = .5f;
    public float maxDistanceRay = 2f;
    private float CalculateMaxDist()
    {
        //float longitudRaycast = minDistanceRay + ((maxDistanceRay - minDistanceRay) * (_player.movement.inAirTime / 5f));
        float longitud = minDistanceRay + (_player.movement.inAirTime * 1.15f);
        float longitudRaycast = Mathf.Clamp(longitud, minDistanceRay, maxDistanceRay);

        return longitudRaycast;
    }
    public RaycastHit GetHit() => hitGround;

    private bool CanApplyDrag() => _player.movement.moveState == Enums.PlayerMovementState.Walking || _player.movement.moveState == Enums.PlayerMovementState.Sprinting;
}
