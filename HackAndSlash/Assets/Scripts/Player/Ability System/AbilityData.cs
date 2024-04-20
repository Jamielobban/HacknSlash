using UnityEngine;

[CreateAssetMenu(menuName = "Ability Data", fileName = "Ability/DataAbility")]
public class AbilityData : ScriptableObject
{
    [Header("Information: ")]
    public Sprite icon;
    public string name;
    public string animation;
    [TextArea] public string description;

    [Header("Variables: ")] 
    public float baseCooldown;
    public float baseCastTime;
    public float baseDamage;

    [Header("References: ")] 
    public AudioSource audio;
    public GameObject effect;

}
