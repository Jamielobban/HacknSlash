using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashAbilityCollider : MonoBehaviour
{
    bool start = false;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        start = true;
        Invoke("End", 1);
    }
    void End()
    {
        this.transform.localPosition = Vector3.zero;
        this.gameObject.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
    }
}
