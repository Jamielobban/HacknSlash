using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Player Data", menuName = "Player/Data")]

public class PlayerData : ScriptableObject
{
    [Header("Combat Stats")]
    [Space]
    [SerializeField]
    public float maxHealth = 100;
    [SerializeField]
    public float critChance;
    public int maxCritChance = 100;
    [SerializeField]
    public float attackDamage;
    [SerializeField]
    public float healthRegen;
    public float timeToHeal = 5f;
    public float critDamageMultiplier;
    public int slowForce;

}
