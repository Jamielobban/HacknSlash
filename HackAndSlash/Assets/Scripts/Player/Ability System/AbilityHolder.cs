using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder : MonoBehaviour
{
    public const int MAX_ABILITIES = 3;
    private int _currentAbilities = 0;
    private BaseAbility _l2Square;
    private BaseAbility _l2Triangle;
    private BaseAbility _l2Circle;

    public Image hudCooldownL2Square, hudCooldownL2Triangle, hudCooldownL2Circle;

    public BaseAbility L2Square => _l2Square;
    public BaseAbility L2Triangle => _l2Triangle;
    public BaseAbility L2Circle => _l2Circle;

    public void AddAbility(BaseAbility ability)
    {
        if (CanAddAbility())
        {
            if (_l2Square == null)
            {
                _l2Square = ability;
                ability.hudCooldown = hudCooldownL2Square;
            }
            else if (_l2Triangle == null)
            {
                _l2Triangle = ability;
                ability.hudCooldown = hudCooldownL2Triangle;
            }
            else
            {
                _l2Circle = ability;
                ability.hudCooldown = hudCooldownL2Circle;
            }

            _currentAbilities++;
        }
    }
    public bool CanAddAbility() => _currentAbilities < MAX_ABILITIES;

}