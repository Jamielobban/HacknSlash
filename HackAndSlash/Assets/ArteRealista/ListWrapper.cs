using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListWrapper<T>
{
    [SerializeField]
    public List<T> collection;
}

[System.Serializable]
public class MultipleListElement<T, T1>
{
    [SerializeField]
    public T key;
    [SerializeField]
    public T1 value;
}

