using System.Collections.Generic;
using UnityEngine;

public class Combo : MonoBehaviour
{
    public List<Enums.InputsAttack> sequence = new List<Enums.InputsAttack>();
    public List<BaseAttack> attack = new List<BaseAttack>();


    public bool IsNextInput(Enums.InputsAttack action,int comboIndex) 
    {
        if (comboIndex < sequence.Count)
            return sequence[comboIndex] == action;
        return false;
    }

    public BaseAttack GetNextAttack(List<Enums.InputsAttack> currentSequence, int currentIndex) 
    {
        if (currentIndex >= sequence.Count)
            return null;
        if (UtilsNagu.ListContainsAnotherListFromStart(sequence, currentSequence))
            return attack[currentIndex];
        else return null;
    }

}
