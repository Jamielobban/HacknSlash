using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Burn, Poison, Bleed, Slow
public class EnemyDebuffSystem : MonoBehaviour
{
    private EnemyBase _enemy;

    public bool isStun;

    public Debuff[] debuffs; // Burn, Poison, Bleed, Slow

    private void Awake()
    {
        _enemy = GetComponent<EnemyBase>();
    }

    private void Update()
    {

    }

}


