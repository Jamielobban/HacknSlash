using DamageNumbersPro;
using System;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damage;
    public DamageNumber visualEffect;

    protected virtual void Awake()
    {
        
    }
    protected virtual void Update()
    {
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>().TakeDamage(damage, visualEffect);
    }
    protected virtual void DestroyGameObject()
    {
    } 
}
