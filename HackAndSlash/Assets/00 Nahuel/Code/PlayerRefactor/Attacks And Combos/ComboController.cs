using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    public List<Combo> combos = new List<Combo>();

    private List<Enums.InputsAttack> _currentSequence = new List<Enums.InputsAttack>();
    public List<Enums.InputsAttack> CurrentSequence => _currentSequence;

    public BaseAttack currentAttack;

    public int currentActionIndex;

    private float _timer;

    private void Update()
    {
        if(_currentSequence.Count >= 1)
        {
            _timer += Time.deltaTime;
            if(currentAttack != null)
            {
                if (_timer >= currentAttack.data.timeToChangeAnim)
                {
                    ConncatenateAttack();
                }
                if(currentAttack != null)
                {
                    if (_timer >= currentAttack.data.animationLength + 0.25f)
                    {
                        ClearComboSequence();
                    }
                }
            }
        }
    }
    public void AddInputToSequence(Enums.InputsAttack action)
    {
        if (IsCorrectInput(action)) 
        {
            _currentSequence.Add(action);
            if (currentActionIndex <= 0)
                DoAttack();
        }
    }
    public void DoAttack() 
    {
        if (_currentSequence.Count <= currentActionIndex)
            return;
        currentAttack = GetNextAttack();
        if (currentAttack != null) 
        {
            _timer = 0;
            currentAttack.Use();
            currentActionIndex++;
        }
    }
    public void ConncatenateAttack() 
    {
        DoAttack();
    }

    private BaseAttack GetNextAttack() 
    {
        foreach (Combo combo in combos)
        {
            BaseAttack attack = combo.GetNextAttack(_currentSequence, currentActionIndex);
            if(attack != null)
                return attack;
        }
        return null;
    }
    private bool IsCorrectInput(Enums.InputsAttack action) 
    {
        foreach (Combo combo in combos)
        {
            if (combo.IsNextInput(action, _currentSequence.Count)) 
            {
                return true;
            }
        }
        return false;
    }
    private void ClearComboSequence()
    {
        _currentSequence.Clear();
        currentActionIndex = 0;
        _timer = 0;
    }

}
