using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpDamageCollider : MonoBehaviour
{
    public int ampDamage;
    public DamageNumber ampDamageNumber;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hello");
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy inside me");
            other.GetComponent<Enemy>().healthSystem.CurrentHealth -= ampDamage;
            DamageNumber ampDamageNumberSpawn = ampDamageNumber.Spawn(other.GetComponent<Enemy>().transform.position, -ampDamage);
        }
    }
}
