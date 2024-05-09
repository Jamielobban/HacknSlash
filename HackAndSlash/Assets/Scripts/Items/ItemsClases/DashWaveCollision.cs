using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashWaveCollision : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Active()
    {
        this.GetComponent<BoxCollider>().enabled = true;
        Invoke("Disable", 0.1f);

    }

    void Disable()
    {
        this.GetComponent<BoxCollider>().enabled = false;
    }
    private void OnEnable()
    {
        Invoke("Active", 0.1f);
    }

}
