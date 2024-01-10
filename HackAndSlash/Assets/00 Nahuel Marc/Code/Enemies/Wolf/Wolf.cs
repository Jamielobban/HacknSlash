using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : Enemy
{
    public CollisionGround collisionGround;

    protected override void Update()
    {
        base.Update();
        if(collisionGround.isGrounded)
        {
            isGrounded = collisionGround.isGrounded;
        }
    }
}
