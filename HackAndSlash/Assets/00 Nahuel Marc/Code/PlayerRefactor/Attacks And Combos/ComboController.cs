using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboController : MonoBehaviour
{
    public List<Combo> combos = new List<Combo>();

    private List<InputAction> _currentSequence = new List<InputAction>();
    public List<InputAction> CurrentSequence => _currentSequence;

    private float _timer = 0f;
    private float _timeToResetInputs = 2f;
    private void Update()
    {
        if(_currentSequence.Count > 0)
        {
            _timer += Time.deltaTime;
            if (_timer >= _timeToResetInputs)
            {
                ClearComboSequence();
                _timer = 0f;
            }
        }
    }

    public void AddInputToSequence(InputAction action)
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
               // _currentSequence.Clear();
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
