using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private PlayerManager _player;
    private Vector3 delayedForceToApply;

    [Header("Dash stats:")]
    public float dashForce;

    private void Awake()
    {
        _player = GetComponent<PlayerManager>();
    }
    public void HandleDash()
    {
        if (_player.isInteracting)
        {
            return;
        }
        _player.animations.PlayTargetAnimation("roll", true);
    }

    public void DashAction(float force)
    {
        _player.rb.velocity = Vector3.zero;
        _player.rb.drag = 0;
        _player.movement.isDashing = true;
        _player.isInvulnerable = true;
        Vector3 forceToApply;

        if (_player.movement.OnSlope())
        {
            forceToApply = _player.movement.GetForwardSlopeDirection() * force;
        }
        else
        {
            forceToApply = transform.forward * force;
        }

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedForceToApply), 0.025f);
    }

    public void Dash(float force)
    {
        _player.rb.velocity = Vector3.zero;
        _player.rb.drag = 0;
        _player.movement.isDashing = true;
        _player.isInvulnerable = true;
        Vector3 forceToApply;
        // si hay un enemigo cerca hace el dash direction enemy y rota hacia el ??
        if (_player.movement.OnSlope())
        {
            forceToApply = _player.movement.GetForwardSlopeDirection() * force;
        }
        else
        {
            forceToApply = transform.forward * force;
        }

        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedForceToApply), 0.025f);
       // Invoke(nameof(ResetDash), dashDuration);
    }

    private void DelayedForceToApply()
    {
        _player.rb.AddForce(delayedForceToApply, ForceMode.Impulse);
    }

    public void ResetDash()
    {
        _player.isInvulnerable = false;
        _player.movement.isDashing = false;
    }

}
