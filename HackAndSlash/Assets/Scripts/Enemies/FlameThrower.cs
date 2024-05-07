using DamageNumbersPro;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    public float damage;
    public float timeToDamage;
    private float _currentTime;
    private bool canDealDamage = false;
    public DamageNumber visualEffect;

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if(_currentTime >= timeToDamage)
        {
            canDealDamage = true;
            _currentTime = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(canDealDamage && other.gameObject != null)
        {
            other.GetComponent<IDamageable>().TakeDamage(damage, visualEffect);
            canDealDamage = false;
        }
    }
}
