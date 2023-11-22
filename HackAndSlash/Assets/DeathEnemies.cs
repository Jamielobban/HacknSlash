using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEnemies : MonoBehaviour
{
    public List<PlayerControl.PassiveCombo> deathEnemiesLastAttack;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetEnemieDeath(PlayerControl.PassiveCombo attack)
    {
        deathEnemiesLastAttack.Add(attack);

        if(deathEnemiesLastAttack.Count > 20)
        {
            deathEnemiesLastAttack.RemoveAt(0);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
