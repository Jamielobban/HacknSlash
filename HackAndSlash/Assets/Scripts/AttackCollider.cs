using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DamageNumbersPro;

public class AttackCollider : MonoBehaviour
{
    PlayerControl _player;
    public PlayerControl.HitState state;    

    void Start()
    {
        _player = FindObjectOfType<PlayerControl>();
    }
    float CalculateDamage()
    {
        float damage = (int)state * _player.stats.attackDamage;

        int rand = Random.Range(0, _player.stats.maxCritChance);
        if (rand < _player.stats.critChance)
        {
            damage *= _player.stats.critDamageMultiplier;
        }
        return damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageableEnemy>()?.TakeDamage(state, CalculateDamage());
    }

}
