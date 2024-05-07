using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.Utility;

public class GemRecovery : MonoBehaviour, IInteractable
{
    public RecoveryFlower _recoveryFlower;
    [SerializeField] BloodAltar bAltar;
    [SerializeField] UnityEvent interactDone;

    private void OnTriggerEnter(Collider other)
    {
        bAltar.ShowObjectInRange();
    }

    private void OnTriggerExit(Collider other)
    {
        bAltar.HideObjectInRange();
    }

    public void Interact()
    {
        PlayerControl player = GameManager.Instance.Player;
        player.healthSystem.Heal(player.healthSystem.maxHealth);
        _recoveryFlower.ResetStructure();
        interactDone.Invoke();
    }

}
