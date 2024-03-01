using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private List<GameObject> _touchingInteractables = new List<GameObject>();
    private List<GameObject> _touchingEnemies = new List<GameObject>();
    public List<GameObject> TouchingEnemies => _touchingEnemies;

    public bool canInteract;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Interactable")
        {
            if(other.GetComponent<Interactive>())
            {
                canInteract = true;
                other.GetComponent<Interactive>().ShowObjectInRange();
            }
            if(!_touchingInteractables.Contains(other.gameObject))
            {
                _touchingInteractables.Add(other.gameObject);
            }
        }
        if (other.gameObject.tag == "Enemy")
        {
            if(!_touchingEnemies.Contains(other.gameObject))
            {
                _touchingEnemies.Add(other.gameObject);
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
        if(other.gameObject.tag == "Enemy")
        {
            if(_touchingEnemies.Contains(other.gameObject))
            {
                _touchingEnemies.Remove(other.gameObject);
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
