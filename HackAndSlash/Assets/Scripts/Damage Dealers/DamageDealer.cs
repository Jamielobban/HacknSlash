using System;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damage;

    protected virtual void Awake()
    {
        
    }
    protected virtual void Update()
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>().TakeDamage(damage);
    }
    protected virtual void DestroyGameObject()
    {
    } 
}
