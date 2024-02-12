using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public DamageAttack attack;
    protected PlayerManager _player;

    protected virtual void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
    }

    protected virtual void Update()
    {
    }

    // --- Ability Damage + Player Damage ?? --- //
    protected virtual void DoDamage(IDamageable entity)
    {
        entity.TakeDamage(attack.baseDamage + _player.stats.baseDamage);
    }

    protected virtual void Stun(IDamageable entity)
    {
       // entity.ApplyStun(ability.timeStun.Value);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
    }
    protected virtual void OnTriggerExit(Collider other)
    {
    }

    protected virtual void DestroyGameObject()
    {

    }
}
