using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboController : MonoBehaviour
{
    public List<Combo> combos = new List<Combo>();

    private List<InputAction> _currentSequence = new List<InputAction>();

    public void AddComboToSequence(InputAction action)
    {
        _currentSequence.Add(action);
    }

    public void ClearComboSequence()
    {
        _currentSequence.Clear();
    }

    public void HandleCombo()
    {
        foreach (var combo in combos)
        {
            if(CheckCombo(combo))
            {
                _currentSequence.Clear();
            }
        }
    }

    public bool CheckCombo(Combo combo)
    {
        if(_currentSequence.Count != combo.sequence.Count)
        {
            return false;
        }

        for (int i = 0; i < _currentSequence.Count; i++)
        {
            if (_currentSequence[i] != combo.sequence[i])
            {
                return false;
            }
        }

        return true;
    }

}
