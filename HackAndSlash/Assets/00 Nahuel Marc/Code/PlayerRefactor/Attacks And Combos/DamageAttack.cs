using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAttack : BaseAttack
{
    [Header("Stats Damage")]
    public float baseDamage;
    public float timeStun; //If it stuns

    protected override void SetVisualEffects()
    {
        base.SetVisualEffects();
        //Instantiate effect with DealDamage
        GameObject go = Instantiate(data.effect, transform.position, Quaternion.identity);
        go.GetComponent<DealDamage>().attack = this;
    }
}
