using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Challenge", menuName = "ChallengeEnemyDeathCount", order = 3)]

public class Challenges2 : Challenges
{
    public int currentCount;

    public int count;

    public bool Check(int i)
    {


        return i >= count;
    }
}