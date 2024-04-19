﻿using System;
using UnityEngine;

public class EnemyInEvent : MonoBehaviour
{
    public EventMap _event;
    private EnemyBase _enemy;

    private void Awake()
    {
        _enemy = GetComponent<EnemyBase>();
    }

    private void Update()
    {
        if (_enemy.HealthSystem.CurrentHealth <= 0)
        {
            _event.RemoveEnemy(gameObject);
        }
    }
}
