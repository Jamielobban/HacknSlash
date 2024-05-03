using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{

    private void OnEnable()
    {
        Invoke(nameof(Heal), 0.2f);
    }

    private void Heal() => GameManager.Instance.Player.healthSystem.Heal(50);
}
