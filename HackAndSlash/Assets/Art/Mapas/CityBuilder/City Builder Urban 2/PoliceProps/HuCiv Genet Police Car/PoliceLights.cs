using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoliceLights : MonoBehaviour
{
    public Texture2D red, blue;
    public float intensityR = 0.4f, intensityB = 0.6f, timeBetweenChange = 1.5f;
    bool blueOn = true;
    float counter = 0;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        counter += Random.Range(0, timeBetweenChange * 2);
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - counter >= timeBetweenChange)
        {
            counter = Time.time;
            if( blueOn )
            {
                blueOn = false;
                mat.SetTexture("_EmissionMap", red);
                mat.SetVector("_EmissionColor", Color.red * intensityR);
            }
            else
            {
                blueOn = true;
                mat.SetTexture("_EmissionMap", blue);
                mat.SetVector("_EmissionColor", Color.blue * intensityB);
            }
        }
    }
}
