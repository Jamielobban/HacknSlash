using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ProtectEvent : EventBaseRounds
{
    [SerializeField] List<Transform> targetsToProtect;
    public GameObject[] activables;
    protected override void Start()
    {
        base.Start();

        foreach (var activable in activables)
        {
            activable.SetActive(false);
        }
    }
 
    protected override void StartEvent()
    {
        base.StartEvent();
        
        foreach (var activable in activables)
        {
            activable.SetActive(true);
        }
        
        foreach (Transform t in targetsToProtect)
        {
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.gameObject.SetActive(true));
        }
        
    }
    protected override void CompleteEvent()
    {
        base.CompleteEvent();

        foreach (EnemiesToKill enemy in roundsOfEnemies)
        {
            foreach (var e in enemy.enemiesToKill)
            {
                e.GetComponent<EnemyBase>().OnDespawnEnemy();
            }
            enemy.enemiesToKill.Clear();
            enemy.parent.gameObject.SetActive(false);
        }
        foreach (var activable in activables)
        {
            activable.SetActive(false);
        }
        foreach (Transform t in targetsToProtect)
        {
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.gameObject.SetActive(false));
        }
    }
  
    protected override void CheckEventState()
    {
        base.CheckEventState();
        if(!AllEnemiesDefeated())
        {
            foreach (Transform t in targetsToProtect)
            {
                List<DamageableObject> damageables = GetComponentsInChildren<DamageableObject>().ToList();
                foreach (DamageableObject dam in damageables)
                {
                    if (dam.IsDead)
                    {
                        break;
                    }
                }
            }
        }
    }    
}
