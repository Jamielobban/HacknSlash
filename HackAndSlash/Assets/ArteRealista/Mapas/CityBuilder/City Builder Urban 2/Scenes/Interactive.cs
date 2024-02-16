using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactive : MonoBehaviour
{
    [SerializeField] Material triggerMat, normalMat;
    bool playerInside = false, canTriggerAgain = true;
    Collider col;

    private void OnEnable()
    {
        col = GetComponent<Collider>();
        int playerLayer = LayerMask.NameToLayer("Player");
        LayerMask allLayersExceptPlayer = ~(1 << playerLayer);
        col.excludeLayers = allLayersExceptPlayer;
        col.includeLayers = (1 << playerLayer);
    }

    

    private void Update()
    {
        if (canTriggerAgain && playerInside && Input.GetKeyDown(KeyCode.H))
            TriggerAction();
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponentInChildren<Renderer>().material = triggerMat;
        playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GetComponentInChildren<Renderer>().material = normalMat;
        playerInside = false;
    }

    protected abstract void TriggerAction();
}
