using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float damage;

    public float knockbackForce = 1000;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>().TakeDamage(damage);
        GameObject player = other.gameObject.transform.parent.gameObject;

        Vector3 dir = (player.transform.position - transform.position).normalized;
        dir.y = 0;
        GameManager.Instance.Player.rb.drag = 15;
        player.GetComponent<Rigidbody>().AddForce(dir * knockbackForce, ForceMode.Impulse);
    }
}
