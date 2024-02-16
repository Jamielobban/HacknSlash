using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private List<GameObject> _touchingInteractables = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Interactable")
        {
            other.GetComponent<Interactive>().ShowObjectInRange();
            if(!_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            other.GetComponent<Interactive>().HideObjectInRange();
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
