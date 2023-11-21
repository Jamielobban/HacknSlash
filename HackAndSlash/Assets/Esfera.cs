using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Esfera : MonoBehaviour
{
    public GameObject bosque;
    public GameObject fuego;
    public GameObject entorno;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerControl>() != null)
        {
            bosque.SetActive(false);
            fuego.SetActive(true);
            entorno.SetActive(false);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerControl>() != null)
        {
            bosque.SetActive(true);
            fuego.SetActive(false);
            entorno.SetActive(true);
        }
    }
}
