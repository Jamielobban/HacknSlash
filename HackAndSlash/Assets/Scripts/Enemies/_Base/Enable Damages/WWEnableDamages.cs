using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WWEnableDamages : EnableDamages
{
    public Collider jumpCollider;

    protected override void Awake()
    {
        base.Awake();
        jumpCollider.enabled = false;
    }

    public void SetJumpCollider()
    {
        jumpCollider.enabled = !jumpCollider.enabled;
    }

    public void DisableJumpCollider()
    {
        jumpCollider.enabled = false;
    }
}
