using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Apagar",0.1f);
    }

    void Apagar()
    {
        if(this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerControl>() != null)
        {
            other.GetComponent<PlayerControl>().GetDamage(damage);

        }
    }
}
