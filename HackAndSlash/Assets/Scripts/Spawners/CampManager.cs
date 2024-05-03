using System.Collections.Generic;
using UnityEngine;

public class CampManager : MonoBehaviour
{
    public List<GameObject> enemiesToKill = new List<GameObject>();
    public Chest chest;

    private void Update()
    {
        if(enemiesToKill.Count <= 0)
        {
            chest.canBeUnlocked = true;
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemiesToKill.Contains(enemy))
        {
            enemiesToKill.Remove(enemy);
        }
    }

}
