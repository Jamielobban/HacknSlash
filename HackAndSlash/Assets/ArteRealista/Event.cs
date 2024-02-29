using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : Interactive, IInteractable
{
    protected Enums.EventState currentState;
    // Start is called before the first frame update
    protected abstract void StartEvent();

    // Update is called once per frame
    protected abstract void Update();

    protected abstract void OnTriggerEnter(Collider other);

    public void Interact()
    {
        if (!canInteract) return;

        StartEvent();
        canInteract = false;
    }
}
