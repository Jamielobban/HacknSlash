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
