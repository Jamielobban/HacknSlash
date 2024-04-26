using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemRecovery : MonoBehaviour, IInteractable
{
    public RecoveryFlower _recoveryFlower;
    public void Interact()
    {
        PlayerControl player = GameManager.Instance.Player;
        player.healthSystem.Heal(player.healthSystem.maxHealth);
        gameObject.SetActive(false);
    }

}
