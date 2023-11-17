using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpExplosion : MonoBehaviour
{
    public IEnumerator AmpExplosionDelay(float delay,int damage)
    {
        yield return new WaitForSeconds(delay);
        this.gameObject.GetComponent<EnemySkeletonSword>().health -= damage;
        DamageNumber ampExplosionNumber = this.gameObject.GetComponent<EnemySkeletonSword>().ampedNumberPrefab.Spawn(this.transform.position, -damage);
    }
}
