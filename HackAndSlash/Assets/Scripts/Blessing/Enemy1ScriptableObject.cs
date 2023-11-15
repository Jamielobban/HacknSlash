using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "New Enemy Scriptable Object", menuName = "Enemy 1 Data", order = 1)]
public class Enemy1ScriptableObject : ScriptableObject
{
    public int weaken;
    public int movespeed;
    public int stun;
}
