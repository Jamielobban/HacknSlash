using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerInventory : ScriptableObject
{
    public event Action<Dictionary<int, Item>> OnUpdated;

    [SerializeField] private List<Item> items;

    [field: SerializeField] public int Size { get; private set; } = 20;

    public void Initialize()
    {
        
    }
}
