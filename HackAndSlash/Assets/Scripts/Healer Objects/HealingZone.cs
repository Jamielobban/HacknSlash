using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingZone : MonoBehaviour
{
    public float timeToHeal;
    public float healAmount;
    private bool isInHealArea = false;
    private float _currentTime = 0f;
    private void Update()
    {
        if(isInHealArea)
        {
            _currentTime += Time.deltaTime;
            if(_currentTime >= timeToHeal)
            {
                GameManager.Instance.Player.healthSystem.Heal(healAmount);
                _currentTime = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInHealArea = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInHealArea = false;
    }
}
