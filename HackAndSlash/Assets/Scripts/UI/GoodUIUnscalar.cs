using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoodUIUnscalar : MonoBehaviour
{
    private Material _mat;
    private void Awake()
    {
        _mat = GetComponent<Image>().material;
    }
    void Start()
    {
        
    }

    void Update()
    {
        _mat.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
