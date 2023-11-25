using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ChallengeTipe {CONSECUTIVE, DEATHATTACK };
public abstract class Challenges : ScriptableObject
{    
    public string description;
    public ChallengeTipe tipe;
    public virtual bool Check()
    {
        return false;
    }

}
[CreateAssetMenu(fileName = "New Challenge", menuName = "ChallengeEnemyDeathAttack", order = 2)]

public class Challenges1 : Challenges
{
    public PlayerControl.PassiveCombo enemies;
    public int count;
    public int currentCount;
    public bool Check(PlayerControl.PassiveCombo attack)
    {

        if(attack == enemies)
        {
            currentCount++;
        }


        return count <= currentCount;
    }
}
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