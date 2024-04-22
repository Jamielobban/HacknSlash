using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder : MonoBehaviour
{
    public const int MAX_ABILITIES = 3;
    private int _currentAbilities = 0;
    private PlayerControl.Ataques _l2Square;
    private PlayerControl.Ataques _l2Triangle;
    private PlayerControl.Ataques _l2Circle;

    public float timeL2Square, timeL2Triangle, timeL2Circle;

    public Image hudCooldownL2Square, hudCooldownL2Triangle, hudCooldownL2Circle;

    public PlayerControl.Ataques L2Square => _l2Square;
    public PlayerControl.Ataques L2Triangle => _l2Triangle;
    public PlayerControl.Ataques L2Circle => _l2Circle;

    private void Awake()
    {
        _l2Square.isEmpty = true;
        _l2Triangle.isEmpty = true;
        _l2Circle.isEmpty = true;
    }

    public void AddAbility(PlayerControl.Ataques ability)
    {
        if (CanAddAbility())
        {
            if (_l2Square.isEmpty)
            {
                _l2Square = ability;
                ability.hudCooldown = hudCooldownL2Square;
                _l2Square.isEmpty = false;
            }
            else if (_l2Triangle.isEmpty)
            {
                _l2Triangle = ability;
                ability.hudCooldown = hudCooldownL2Triangle;
                _l2Triangle.isEmpty = false;
            }
            else
            {
                _l2Circle = ability;
                ability.hudCooldown = hudCooldownL2Circle;
                _l2Circle.isEmpty = false;
            }

            _currentAbilities++;
        }
    }
    public bool CanAddAbility() => _currentAbilities < MAX_ABILITIES;

}