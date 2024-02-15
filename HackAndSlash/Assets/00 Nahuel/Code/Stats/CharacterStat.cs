using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class CharacterStat
{
    public float baseValue;
    private bool reCalculate;
    private float _value;
    private float _lastBaseValue = float.MinValue;
    private readonly List<StatModifier> statsModifiers;
    private readonly ReadOnlyCollection<StatModifier> readStatModifiers;
    
    public float Value
    {
        get
        {
            if(reCalculate || baseValue != _lastBaseValue)
            {
                _lastBaseValue = baseValue;
                _value = CalculateFinalValue();
                reCalculate = false;
            }
            return _value;
        }
    }

    public CharacterStat()
    {
        statsModifiers = new List<StatModifier>();
        readStatModifiers = statsModifiers.AsReadOnly();
    }

    public CharacterStat(float v) : this()
    {
        baseValue = v;
    }

    private int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.order < b.order)
            return -1;
        else if (a.order > b.order)
            return 1;
        return 0;
    }

    public bool RemoveModifier(StatModifier mod)
    {
        if(statsModifiers.Remove(mod))
        {
            reCalculate = true;
            return true;
        }
        return false;
    }

    public void AddModifier(StatModifier mod)
    {
        reCalculate = true;
        statsModifiers.Add(mod);
        statsModifiers.Sort(CompareModifierOrder);
    }
    public bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;
        for(int i = statsModifiers.Count -1; i>=0; i--)
        {
            if (statsModifiers[i].source == source)
            {
                reCalculate = true;
                didRemove = true;
                statsModifiers.RemoveAt(i);
            }
        }
        return didRemove;
    }

    public float CalculateFinalValue()
    {
        float finalValue = baseValue;
        float sumPercentAdd = 0;

        for(int i = 0; i < statsModifiers.Count; i++)
        {
            StatModifier mod = statsModifiers[i];

            if (mod.type == StatModifierType.Flat)
            {
                finalValue += mod.value;
            }
            else if (mod.type == StatModifierType.PercentAdd)
            {
                sumPercentAdd += mod.value;
                if (i + 1 >= statsModifiers.Count || statsModifiers[i + 1].type != StatModifierType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if (mod.type == StatModifierType.PercentMult)
            {
                finalValue *= 1 + mod.value;
            }
        }
        return (float)Math.Round(finalValue, 4);
    }
}
