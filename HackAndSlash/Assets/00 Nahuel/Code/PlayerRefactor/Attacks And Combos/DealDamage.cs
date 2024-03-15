using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    public float damage;
    public bool canStun;
    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
    }

    protected virtual void Stun(IDamageable entity)
    {
       // entity.ApplyStun(ability.timeStun.Value);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageableEntity = other.GetComponent<IDamageable>();

        if (damageableEntity != null)
        {
            damageableEntity.TakeDamage(damage);

            if(canStun)
            {
                //Stun
            }

        }
    }
    protected virtual void OnTriggerExit(Collider other)
    {
    }

    protected virtual void DestroyGameObject()
    {

    }
}
