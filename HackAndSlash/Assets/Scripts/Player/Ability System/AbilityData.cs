using UnityEngine;

[CreateAssetMenu(fileName = "Ability Data", menuName = "Items/DataAbility")]
public class AbilityData : ItemData
{
    public string animation;

    [Header("Variables: ")] 
    public float baseCooldown;
    public float baseCastTime;
    public float baseDamage;
    public AnimationCurve curvaDeVelocidadMovimiento;
    public float velocidadMovimiento;

    public AnimationCurve curvaDeVelocidadMovimientoY;
    public float velocidadMovimientoY;
    
    
    [Header("References: ")] 
    public AudioSource audio;
    public GameObject effect;

}
