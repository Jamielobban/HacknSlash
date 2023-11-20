using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmpExplosion : MonoBehaviour
{
    public GameObject chargeAmp;
    public GameObject chargeDamagePrefab;
    public IEnumerator AmpExplosionDelay()
    {
        GameObject chargeAmpPrefab = Instantiate(chargeAmp, this.GetComponent<Transform>());
        yield return new WaitForSeconds(4f);
        Destroy(chargeAmpPrefab);
        GameObject chargeObject = Instantiate(chargeDamagePrefab, this.GetComponent<Transform>());
        StartCoroutine(DisableCollider(0.1f,chargeObject));
        //DamageNumber ampExplosionNumber = this.gameObject.GetComponent<EnemySkeletonSword>().ampedNumberPrefab.Spawn(this.transform.position, -damage);
    }

    private IEnumerator DisableCollider(float delay, GameObject something)
    {
        yield return new WaitForSeconds(delay);
        something.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(something);
    }
}
