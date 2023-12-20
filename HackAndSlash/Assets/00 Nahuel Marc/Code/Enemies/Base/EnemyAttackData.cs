using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackData", menuName = "Enemies/Attacks")]
public class EnemyAttackData : ScriptableObject
{
    [Header("Attack info")]
    public string attackName;
    public string animation;

    [Header("Base Stats")]
    public CharacterStat cooldown = new CharacterStat();
    public CharacterStat castTime = new CharacterStat();
    public CharacterStat damage = new CharacterStat();
    public CharacterStat range = new CharacterStat();
}
