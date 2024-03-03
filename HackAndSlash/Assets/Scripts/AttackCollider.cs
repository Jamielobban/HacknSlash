using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DamageNumbersPro;

public class AttackCollider : MonoBehaviour
{
    public PlayerControl.HitState state;

    

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {

        }
    }

}
