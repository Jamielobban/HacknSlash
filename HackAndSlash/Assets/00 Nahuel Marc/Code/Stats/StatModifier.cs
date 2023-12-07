using System;
using UnityEngine;

public enum StatModifierType { Flat = 100, PercentAdd = 200, PercentMult = 300 }

[Serializable]
public class StatModifier
{
    public StatModifierType type;
    [SerializeField] public float value;
    public readonly int order;
    public readonly object source;
    public StatModifier(float val, StatModifierType _type, int _order, object _source)
    {
        value = val;
        type = _type;
        order = _order;
        source = _source;
    }

    public StatModifier(float val, StatModifierType _type) : this(val, _type, (int)_type, null) { }
    public StatModifier(float val, StatModifierType _type, int _order) : this(val, _type, _order, null) { }
    public StatModifier(float val, StatModifierType _type, object _source) : this(val, _type, (int)_type, _source) { }

}
