using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combo : MonoBehaviour
{
    public List<InputAction> sequence = new List<InputAction>();
    public List<BaseAttack> attack = new List<BaseAttack>();
}
