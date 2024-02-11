using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private List<GameObject> _touchingInteractables = new List<GameObject>();
    private PlayerManager _player;
    private bool _isGrounded = false;
    public bool IsGrounded => _isGrounded;

    //public float raycastHeightOffset;
    //public LayerMask groundLayer;
    //public float maxDistance;
    private void Awake()
    {
        //_player = transform.parent.GetComponent<PlayerManager>();
    }

    //public void HandleGroundCollision()
    //{
    //    RaycastHit hit;
    //    Vector3 raycastOrigin = transform.position;
    //    Vector3 targetPosition;
    //    raycastOrigin.y = raycastOrigin.y + raycastHeightOffset;
    //    targetPosition = transform.position;

    //    if (Physics.Raycast(raycastOrigin, -Vector3.up, out hit, maxDistance, groundLayer))
    //    {
    //        if (!_isGrounded && !_player.isInteracting)
    //        {
    //            _player.animations.OnLandingAnimation();
    //            _player.movement.EnableMovement();
    //        }
    //        targetPosition.y = hit.point.y;
    //        _isGrounded = true;
    //    }
    //    else
    //    {
    //        if (!_player.isInteracting)
    //        {
    //            _player.animations.OnFallingAnimation();
    //        }
    //        _isGrounded = false;
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Interactable")
        {
            if(!_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            if (_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Remove(other.gameObject);
            }
        }
    }

    public void InteractPerformed()
    {
        foreach (var interactable in _touchingInteractables)
        {
            if(interactable != null)
            {
                var interaction = interactable.GetComponent<IInteractable>();
                if(interaction == null) { return; }
                interaction.Interact();
            }
        }
    }
}
