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
    public Image hudCooldownL2SquareParent, hudCooldownL2TriangleParent, hudCooldownL2CircleParent;

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
                hudCooldownL2SquareParent.sprite = ability.spriteAbility;
                hudCooldownL2Square.sprite = ability.spriteAbility;
                _l2Square.isEmpty = false;
            }
            else if (_l2Triangle.isEmpty)
            {
                _l2Triangle = ability;
                hudCooldownL2TriangleParent.sprite = ability.spriteAbility;
                hudCooldownL2Triangle.sprite = ability.spriteAbility;

                _l2Triangle.isEmpty = false;
            }
            else
            {
                _l2Circle = ability;
                hudCooldownL2CircleParent.sprite = ability.spriteAbility;
                hudCooldownL2Circle.sprite = ability.spriteAbility;

                _l2Circle.isEmpty = false;
            }

            _currentAbilities++;
        }
    }

    private void Update()
    {
        if(!_l2Square.isEmpty)
        {
            if ((Time.time - timeL2Square) > _l2Square.baseCooldown)
            {
                hudCooldownL2Square.fillAmount = 0;
            }
            else
            {
                UpdateHudL2SquareCooldown(Time.time - timeL2Square);
            }
        }
       if(!_l2Triangle.isEmpty)
       {
            if ((Time.time - timeL2Triangle) > _l2Triangle.baseCooldown)
            {
                hudCooldownL2Triangle.fillAmount = 0;
            }
            else
            {
                UpdateHudL2TriangleCooldown(Time.time - timeL2Triangle);

            }
        }

       if(!_l2Circle.isEmpty)
       {
            if ((Time.time - timeL2Circle) > _l2Circle.baseCooldown)
            {
                hudCooldownL2Circle.fillAmount = 0;
            }
            else
            {
                UpdateHudL2CircleCooldown(Time.time - timeL2Circle);

            }
        }

    }

    public void UpdateHudL2SquareCooldown(float time)
    {
        float progress = Mathf.Clamp01(1 - (time / L2Square.baseCooldown));
        hudCooldownL2Square.fillAmount = progress;
    }

    public void UpdateHudL2TriangleCooldown(float time)
    {
        float progress = Mathf.Clamp01(1 - (time / L2Triangle.baseCooldown));
        hudCooldownL2Triangle.fillAmount = progress;
    }

    public void UpdateHudL2CircleCooldown(float time)
    {
        float progress = Mathf.Clamp01(1 - (time / L2Circle.baseCooldown));
        hudCooldownL2Circle.fillAmount = progress;
    }

    public bool CanAddAbility() => _currentAbilities < MAX_ABILITIES;

}