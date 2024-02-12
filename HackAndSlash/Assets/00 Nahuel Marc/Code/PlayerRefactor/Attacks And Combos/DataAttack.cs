using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack Data", menuName = "Attacks/Data")]
public class DataAttack : ScriptableObject
{
    [Header("Attack Info")]
    public string attackName;
    public string animation;

    [Header("States")]
    public List<Enums.CharacterState> AllowedCharacterStates = new List<Enums.CharacterState>();

    [Header("Base Stats")]
    public float cooldown;
    public float castTime;

    [Header("References")]
    public GameObject effect;
    public AudioSource audio;
}
