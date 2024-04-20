using System;
using UnityEngine;

public class CampSpawnerEnemy : MonoBehaviour
{
    public CampManager _campParent;
    private EnemyBase _enemy;

    private void Awake()
    {
        _enemy = GetComponent<EnemyBase>();
    }

    private void Update()
    {
        if (_enemy.HealthSystem.CurrentHealth <= 0)
        {
            _campParent.RemoveEnemy(gameObject);
        }
    }
}
