using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Challenge", menuName = "ChallengeEnemyDeathAttack", order = 2)]

public class Challenges1 : Challenges
{
    public PlayerControl.PassiveCombo enemies;
    public int count;
    public int currentCount;
    public bool Check(PlayerControl.PassiveCombo attack)
    {

        if (attack == enemies)
        {
            currentCount++;
        }


        return count <= currentCount;
    }
}
