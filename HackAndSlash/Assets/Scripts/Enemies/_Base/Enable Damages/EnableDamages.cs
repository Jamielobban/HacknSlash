using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDamages : MonoBehaviour
{
    public Collider meleeDamages;

    protected virtual void Awake()
    {
        meleeDamages.enabled = false;
    }

    public void SetMeleeCollider()
    {
        meleeDamages.enabled = !meleeDamages.enabled;
    }

    public void DisableMeleeCollider()
    {
        meleeDamages.enabled = false;
    }
}
