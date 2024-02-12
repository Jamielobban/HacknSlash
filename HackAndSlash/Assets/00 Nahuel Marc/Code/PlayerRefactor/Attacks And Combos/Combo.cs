using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Attack Data", menuName = "Attacks/Combo")]
public class Combo : ScriptableObject
{
    public List<InputAction> sequence = new List<InputAction>();
  //  public List<DataAttack> attack;
}
