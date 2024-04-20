using System;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    private BaseAbility _l2Square;
    private BaseAbility _l2Triangle;
    private BaseAbility _l2Circle;

    public BaseAbility L2Square => _l2Square;
    public BaseAbility L2Triangle => _l2Triangle;
    public BaseAbility L2Circle => _l2Circle;
    
    public void SetSquareAbility(BaseAbility ability) => _l2Square = ability;
    public void SetTriangleAbility(BaseAbility ability) => _l2Triangle = ability;
    public void SetCircleAbility(BaseAbility ability) => _l2Circle = ability;
    
}
