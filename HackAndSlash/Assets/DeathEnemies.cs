using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemies : MonoBehaviour
{
    ChallengeManager manager;
    public List<PlayerControl.PassiveCombo> deathEnemiesLastAttack;
    public float deathTime;
    public int consecutiveDeaths;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindObjectOfType<ChallengeManager>();
        consecutiveDeaths = 0;
    }
    public void SetEnemieDeath(PlayerControl.PassiveCombo attack)
    {
        deathEnemiesLastAttack.Add(attack);
        deathTime = Time.time;
        consecutiveDeaths++;

        for(int i = 0; i < manager.currentChallenges.Count;i++)
        {
            switch (manager.currentChallenges[i].tipe)
            {
                case ChallengeTipe.CONSECUTIVE:
                    if (((Challenges2)manager.currentChallenges[i]).Check(consecutiveDeaths))
                    {
                        manager.ChallengeComplete(i);
                    }
                    break;
                case ChallengeTipe.DEATHATTACK:
                    if (((Challenges1)manager.currentChallenges[i]).Check(attack))
                    {
                        manager.ChallengeComplete(i);
                    }
                    break;
            }

        }

        if (deathEnemiesLastAttack.Count > 20)
        {
            deathEnemiesLastAttack.RemoveAt(0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if((Time.time-deathTime) > 5)
        {
            consecutiveDeaths = 0;

        }
    }
}
