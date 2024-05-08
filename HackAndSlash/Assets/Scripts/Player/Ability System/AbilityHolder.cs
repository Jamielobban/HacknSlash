using System;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHolder : MonoBehaviour
{
    private AbilityItem _l2Square;
    private AbilityItem _l2Triangle;
    private AbilityItem _holdSquare;
    private AbilityItem _holdTriangle;

    public float timeL2Square, timeL2Triangle;
    public float timeHoldSquare, timeHoldTriangle;

    public Image hudCooldownL2Square, hudCooldownL2Triangle;
    public Image hudCooldownHoldSquare, hudCooldownHoldTriangle;

    public Image hudCooldownL2SquareParent, hudCooldownL2TriangleParent;
    public Image hudCooldownHoldSquareParent, hudCooldownHoldTriangleParent;

    public GameObject[] abilitiesParent;

    public AbilityItem L2Square => _l2Square;
    public AbilityItem L2Triangle => _l2Triangle;
    public AbilityItem HoldSquare => _holdSquare;
    public AbilityItem HoldTriangle => _holdTriangle;

    private void Awake()
    {
        if(_l2Square != null)
            _l2Square.ability.isEmpty = true;
        if(_l2Triangle != null)
            _l2Triangle.ability.isEmpty = true;
        if(_holdSquare != null)
            _holdSquare.ability.isEmpty = true;
        if(_holdTriangle != null)
            _holdTriangle.ability.isEmpty = true;
    }

    public void AddAbility(Enums.AbilityInput input, AbilityItem ability)
    {
        if (_l2Square == null && input == Enums.AbilityInput.L2Square)
        {
            _l2Square = ability;
            hudCooldownL2SquareParent.sprite = ability.ability.spriteAbility;
            hudCooldownL2Square.sprite = ability.ability.spriteAbility;
            _l2Square.ability.isEmpty = false;
            abilitiesParent[0].SetActive(true);
        }
        else if (_l2Triangle == null && input == Enums.AbilityInput.L2Triangle)
        {
            _l2Triangle = ability;
            hudCooldownL2TriangleParent.sprite = ability.ability.spriteAbility;
            hudCooldownL2Triangle.sprite = ability.ability.spriteAbility;
            _l2Triangle.ability.isEmpty = false;
            abilitiesParent[1].SetActive(true);
        }
        else if (_holdSquare == null && input == Enums.AbilityInput.HoldSquare)
        {
            _holdSquare = ability;
            hudCooldownHoldSquareParent.sprite = ability.ability.spriteAbility;
            hudCooldownHoldSquare.sprite = ability.ability.spriteAbility;
            _holdSquare.ability.isEmpty = false;
            abilitiesParent[2].SetActive(true);
        }
        else if (_holdTriangle == null && input == Enums.AbilityInput.HoldTriangle)
        {
            _holdTriangle = ability;
            hudCooldownHoldTriangleParent.sprite = ability.ability.spriteAbility;
            hudCooldownHoldTriangle.sprite = ability.ability.spriteAbility;
            _holdTriangle.ability.isEmpty = false;
            abilitiesParent[3].SetActive(true);
        }

    }

    private void Update()
    {
        CheckCooldowns();
    }

    public void UpdateHudL2SquareCooldown(float time) =>
        hudCooldownL2Square.fillAmount = Mathf.Clamp01(1 - (time / _l2Square.ability.baseCooldown));       

    public void UpdateHudL2TriangleCooldown(float time) =>
        hudCooldownL2Triangle.fillAmount = Mathf.Clamp01(1 - (time / _l2Triangle.ability.baseCooldown));
    public void UpdateHudHoldSquareCooldown(float time) =>
        hudCooldownHoldSquare.fillAmount = Mathf.Clamp01(1 - (time / _holdSquare.ability.baseCooldown));
    public void UpdateHudHoldTriangleCooldown(float time) =>
        hudCooldownHoldTriangle.fillAmount = Mathf.Clamp01(1 - (time / _holdTriangle.ability.baseCooldown));
    public bool CanAddAbility(Enums.AbilityInput input)
    {
        switch (input)
        {
            case Enums.AbilityInput.HoldSquare:
                if(_holdSquare == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Enums.AbilityInput.HoldTriangle:
                if (_holdTriangle == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Enums.AbilityInput.L2Square:
                if (_l2Square == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case Enums.AbilityInput.L2Triangle:
                if (_l2Triangle == null)
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
        if(_holdSquare == null)
        {
             return true;

        }
        if(_holdTriangle == null)
        {
             return true;
        }
        if(_l2Square == null)
             return true;
        if(_l2Triangle == null)
             return true;
        return false;
    }

    private void CheckCooldowns()
    {
        if(_l2Square != null)
        {
            if (!_l2Square.ability.isEmpty)
            {
                if ((Time.time - timeL2Square) > _l2Square.ability.baseCooldown)
                {
                    hudCooldownL2Square.fillAmount = 0;
                }
                else
                {
                    UpdateHudL2SquareCooldown(Time.time - timeL2Square);
                }
            }
        }
        if (_l2Triangle != null)
        {
            if (!_l2Triangle.ability.isEmpty)
            {
                if ((Time.time - timeL2Triangle) > _l2Triangle.ability.baseCooldown)
                {
                    hudCooldownL2Triangle.fillAmount = 0;
                }
                else
                {
                    UpdateHudL2TriangleCooldown(Time.time - timeL2Triangle);
                }
            }
        }
        if(_holdSquare != null)
        {
            if (!_holdSquare.ability.isEmpty)
            {
                if ((Time.time - timeHoldSquare) > _holdSquare.ability.baseCooldown)
                {
                    hudCooldownHoldSquare.fillAmount = 0;
                }
                else
                {
                    UpdateHudHoldSquareCooldown(Time.time - timeHoldSquare);
                }
            }
        }

        if(_holdTriangle != null)
        {
            if (!_holdTriangle.ability.isEmpty)
            {
                if ((Time.time - timeHoldTriangle) > _holdTriangle.ability.baseCooldown)
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

}