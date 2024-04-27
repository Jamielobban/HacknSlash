using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo360 : MonoBehaviour
{
    bool move;
    public GameObject[] misiles;
    Vector3[] startPos;
    float timeStartMove;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3[misiles.Length];
        for (int i = 0; i < misiles.Length;i++)
        {
            startPos[i] = misiles[i].transform.localPosition;

        }

        move = false;
        Invoke("StartMoving", 0.75f);
    }
    private void OnEnable()
    {
        // Aqu� colocas el c�digo que deseas que se ejecute cuando el objeto se activa
        Invoke("StartMoving", 0.75f);        // Puedes hacer cualquier otra acci�n aqu�, como cambiar propiedades, iniciar una animaci�n, etc.
    }
    void StartMoving()
    {
        move = true;
        timeStartMove = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            if((Time.time- timeStartMove) > 4)
            {
                move = false;

                for (int i = 0; i < misiles.Length; i++)
                {
                    misiles[i].transform.localPosition = startPos[i];

                }
            }

            for (int i = 0; i < misiles.Length; i++)
            {
                misiles[i].transform.localPosition += misiles[i].transform.forward * speed;

            }
        }
    }
}
