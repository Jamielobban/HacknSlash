using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : DealDamage
{

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().TakeDamage(attack.baseDamage);
        }
    }
}
