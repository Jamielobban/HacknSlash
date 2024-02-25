using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private List<GameObject> _touchingInteractables = new List<GameObject>();
    public bool canInteract;
    private void OnTriggerEnter(Collider other)
    {
        canInteract = true;
        if(other.gameObject.tag == "Interactable")
        {
            if(other.GetComponent<Interactive>())
            {
                other.GetComponent<Interactive>().ShowObjectInRange();
            }
            if(!_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
        if (other.gameObject.tag == "Interactable")
        {
            if (other.GetComponent<Interactive>())
            {
                other.GetComponent<Interactive>().HideObjectInRange();
            }
            if (_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Remove(other.gameObject);
            }
        }
    }

    public void InteractPerformed()
    {
        foreach (var interactable in _touchingInteractables)
        {
            if(interactable != null)
            {
                var interaction = interactable.GetComponent<IInteractable>();
                if(interaction == null) { return; }
                interaction.Interact();
            }
        }
    }
}
