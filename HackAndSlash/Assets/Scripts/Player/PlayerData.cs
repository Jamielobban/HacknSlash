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
    public float stealLifePercentage;

    public AbilityItem item, item2, item3, item4;

    public void ResetData()
    {
        maxHealth = 100;
        critChance = 5;
        maxCritChance = 100;
        attackDamage = 1;
        healthRegen = 0;
        timeToHeal = 5;
        critDamageMultiplier = 2;
        slowForce = 0;
        stealLifePercentage = 0;
        item = null;
        item2 = null;
        item3 = null;
        item4 = null;
    }
    
}
