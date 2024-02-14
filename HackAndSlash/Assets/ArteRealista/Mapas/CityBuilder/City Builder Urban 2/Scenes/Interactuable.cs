using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactuable : MonoBehaviour
{
    [SerializeField] Material triggerMat, normalMat;
    Material objectMaterial;
    bool playerInside = false, canTriggerAgain = true;

    private void Start()
    {
        objectMaterial = GetComponentInChildren<Renderer>().material;
    }

    private void Update()
    {
        if (canTriggerAgain && playerInside && Input.GetKeyDown(KeyCode.E))
            TriggerAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        objectMaterial = triggerMat;
        playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        objectMaterial = normalMat;
        playerInside = false;
    }

    protected abstract void TriggerAction();
}
