using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder : MonoBehaviour
{
    private PlayerControl.Ataques _l2Square;
    private PlayerControl.Ataques _l2Triangle;
    private PlayerControl.Ataques _holdSquare;
    private PlayerControl.Ataques _holdTriangle;

    public float timeL2Square, timeL2Triangle;
    public float timeHoldSquare, timeHoldTriangle;

    public Image hudCooldownL2Square, hudCooldownL2Triangle;
    public Image hudCooldownHoldSquare, hudCooldownHoldTriangle;

    public Image hudCooldownL2SquareParent, hudCooldownL2TriangleParent;
    public Image hudCooldownHoldSquareParent, hudCooldownHoldTriangleParent;

    public GameObject[] abilitiesParent;

    public PlayerControl.Ataques L2Square => _l2Square;
    public PlayerControl.Ataques L2Triangle => _l2Triangle;
    public PlayerControl.Ataques HoldSquare => _holdSquare;
    public PlayerControl.Ataques HoldTriangle => _holdTriangle;

    private void Awake()
    {
        _l2Square.isEmpty = true;
        _l2Triangle.isEmpty = true;
        _holdSquare.isEmpty = true;
        _holdTriangle.isEmpty = true;
    }

    public void AddAbility(Enums.AbilityInput input, PlayerControl.Ataques ability)
    {
        if (_l2Square.isEmpty && input == Enums.AbilityInput.L2Square)
        {
            _l2Square = ability;
            hudCooldownL2SquareParent.sprite = ability.spriteAbility;
            hudCooldownL2Square.sprite = ability.spriteAbility;
            _l2Square.isEmpty = false;
            abilitiesParent[0].SetActive(true);
        }
        else if (_l2Triangle.isEmpty && input == Enums.AbilityInput.L2Triangle)
        {
            _l2Triangle = ability;
            hudCooldownL2TriangleParent.sprite = ability.spriteAbility;
            hudCooldownL2Triangle.sprite = ability.spriteAbility;
            _l2Triangle.isEmpty = false;
            abilitiesParent[1].SetActive(true);
        }
        else if (_holdSquare.isEmpty && input == Enums.AbilityInput.HoldSquare)
        {
            _holdSquare = ability;
            hudCooldownHoldSquareParent.sprite = ability.spriteAbility;
            hudCooldownHoldSquare.sprite = ability.spriteAbility;
            _holdSquare.isEmpty = false;
            abilitiesParent[2].SetActive(true);
        }
        else if (_holdTriangle.isEmpty && input == Enums.AbilityInput.HoldTriangle)
        {
            _holdTriangle = ability;
            hudCooldownHoldTriangleParent.sprite = ability.spriteAbility;
            hudCooldownHoldTriangle.sprite = ability.spriteAbility;
            _holdTriangle.isEmpty = false;
            abilitiesParent[3].SetActive(true);
        }

    }

    private void Update()
    {
        CheckCooldowns();
    }

    public void UpdateHudL2SquareCooldown(float time) =>
        hudCooldownL2Square.fillAmount = Mathf.Clamp01(1 - (time / _l2Square.baseCooldown));       

    public void UpdateHudL2TriangleCooldown(float time) =>
        hudCooldownL2Triangle.fillAmount = Mathf.Clamp01(1 - (time / _l2Triangle.baseCooldown));
    public void UpdateHudHoldSquareCooldown(float time) =>
        hudCooldownHoldSquare.fillAmount = Mathf.Clamp01(1 - (time / _holdSquare.baseCooldown));
    public void UpdateHudHoldTriangleCooldown(float time) =>
        hudCooldownHoldTriangle.fillAmount = Mathf.Clamp01(1 - (time / _holdTriangle.baseCooldown));
    public bool CanAddAbility(Enums.AbilityInput input)
    {
        switch (input)
        {
            case Enums.AbilityInput.HoldSquare:
                if(_holdSquare.isEmpty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Enums.AbilityInput.HoldTriangle:
                if (_holdTriangle.isEmpty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Enums.AbilityInput.L2Square:
                if (_l2Square.isEmpty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Enums.AbilityInput.L2Triangle:
                if (_l2Triangle.isEmpty)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }

    public bool AnyAbilityEmpty()
    {
        if (_holdSquare.isEmpty) return true;
        if (_holdTriangle.isEmpty) return true;
        if (_l2Square.isEmpty) return true;
        if (_l2Triangle.isEmpty) return true;
        return false;
    }

    private void CheckCooldowns()
    {
        if (!_l2Square.isEmpty)
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
        if (!_l2Triangle.isEmpty)
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

        if (!_holdSquare.isEmpty)
        {
            if ((Time.time - timeHoldSquare) > _holdSquare.baseCooldown)
            {
                hudCooldownHoldSquare.fillAmount = 0;
            }
            else
            {
                UpdateHudHoldSquareCooldown(Time.time - timeHoldSquare);
            }
        }
        if (!_holdTriangle.isEmpty)
        {
            if ((Time.time - timeHoldTriangle) > _holdTriangle.baseCooldown)
            {
                hudCooldownHoldTriangle.fillAmount = 0;
            }
            else
            {
                UpdateHudHoldTriangleCooldown(Time.time - timeHoldTriangle);
            }
        }
    }

}