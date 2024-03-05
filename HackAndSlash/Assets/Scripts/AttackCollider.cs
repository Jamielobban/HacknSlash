using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DamageNumbersPro;

public class AttackCollider : MonoBehaviour
{
    PlayerData _player;
    public PlayerControl.HitState state;

    

    void Start()
    {
        _player = FindObjectOfType<PlayerData>();
    }
    float CalculateDamage()
    {
        float damage = (int)state * _player.attackDamage;

        int rand = Random.Range(0, _player.maxCritChance);
        if (rand < _player.critChance)
        {
            damage *= _player.critDamageMultiplier;
        }
        return damage;
    }
    private void OnTriggerEnter(Collider other)
    {




        other.GetComponent<IDamageableEnemy>()?.TakeDamage(state, CalculateDamage());
    }

}
