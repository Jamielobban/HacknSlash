using DamageNumbersPro;
using System;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damage;
    public DamageNumber visualEffect;

    public Collider colliderToEnable;
    public float timeToEnableCollider;

    private float _currentTime;
    public bool canUseCollider = false;

    protected virtual void Awake()
    {
        if(canUseCollider)
            colliderToEnable = GetComponent<Collider>();
    }
    protected virtual void Update()
    {
        _currentTime += Time.deltaTime;
        if(canUseCollider)
        {
            if (_currentTime >= timeToEnableCollider && !colliderToEnable.enabled)
            {
                colliderToEnable.enabled = true;
            }
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>().TakeDamage(damage, visualEffect);
    }
    protected virtual void DestroyGameObject()
    {
    } 
}
