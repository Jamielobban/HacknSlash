using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private List<GameObject> _touchingInteractables = new List<GameObject>();
    public bool canInteract;

    public void ClearInteractables() => _touchingInteractables.Clear();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            if (other.GetComponent<Interactive>())
            {
                canInteract = true;
                other.GetComponent<Interactive>().ShowObjectInRange();
            }
            if (!_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            canInteract = false;
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
