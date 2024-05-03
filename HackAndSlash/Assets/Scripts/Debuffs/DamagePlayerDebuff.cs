using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerDebuff : Debuff
{
    public PlayerControl player;

    public float timeToDamage;
    protected float _damage;

    protected float currentTickTime = 0f;
    public DamageNumber damageNumber;

    void Start()
    {

    }
    protected override void IsActiveUpdate()
    {
        currentTickTime += Time.deltaTime;

        base.IsActiveUpdate();

        if (currentTickTime >= timeToDamage)
        {
            player.healthSystem.TakeDamage(_damage, damageNumber);
            currentTickTime = 0f;
        }
    }

    public void SetDamage(float value) => _damage = value;
}
