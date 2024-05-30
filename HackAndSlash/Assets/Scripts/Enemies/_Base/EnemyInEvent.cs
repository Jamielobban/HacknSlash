using System;
using UnityEngine;

public class EnemyInEvent : MonoBehaviour
{
    public EventBaseRounds _event;
    private EnemyBase _enemy;

    private void Awake()
    {
        _enemy = GetComponent<EnemyBase>();
    }

    private void Update()
    {
        if (_enemy.IsDead)
        {
            _event.RemoveEnemy(gameObject);
        }
    }
}
