using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Attack Data", menuName = "Attacks/Data")]
public class DataAttack : ScriptableObject
{
    [Header("Attack Info")]
    public string attackName;
    public string animation;
    public float timeToChangeAnim;
    public float animationLength;
    
    [Header("States")]
    public List<Enums.CharacterState> AllowedCharacterStates = new List<Enums.CharacterState>();

    [Header("References")]
    public AudioSource audio;
}
