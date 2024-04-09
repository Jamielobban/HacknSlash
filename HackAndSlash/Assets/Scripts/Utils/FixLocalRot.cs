using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixLocalRot : MonoBehaviour
{
    private Quaternion initialRotation;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = initialRotation;
    }
}
