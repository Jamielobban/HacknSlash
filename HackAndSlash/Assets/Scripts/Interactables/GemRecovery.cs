using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemRecovery : Interactive, IInteractable
{
    public RecoveryFlower _recoveryFlower;
    public void Interact()
    {
        if (!GetCanInteract) return;

        PlayerControl player = GameManager.Instance.Player;
        player.healthSystem.Heal(player.healthSystem.maxHealth);
        _recoveryFlower.ResetStructure();
    }

}
