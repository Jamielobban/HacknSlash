using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remate : MonoBehaviour
{
    public float deltaTime;
    bool a;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void remate()
    {
        a = true;
        this.GetComponent<Animator>().CrossFadeInFixedTime("Attack", 0.1f);
    }
    public void endas()
    {
        a = false;

    }
    // Update is called once per frame
    void Update()
    {
        if(a)
        {
            Time.timeScale = deltaTime;
        }
    }
}
