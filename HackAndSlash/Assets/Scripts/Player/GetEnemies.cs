using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEnemies : MonoBehaviour
{
    public List<GameObject> enemies;
    //void Start()
    //{
    //    Invoke("UpdateList", 0.5f);

    //}
    //void UpdateList()
    //{
    //    for (int i = 0; i < enemies.Count; i++)
    //    {
    //        if (enemies[i] == null || enemies[i].tag != "Enemy" || !enemies[i].activeSelf)
    //        {
    //            enemies.RemoveAt(i);
    //        }
    //        if (enemies[i].transform.parent != null)
    //        {
    //            if (!enemies[i].transform.parent.gameObject.activeSelf)
    //            {
    //                enemies.RemoveAt(i);

    //            }
    //        }
    //    }
    //    Invoke("UpdateList", 0.5f);
    //}
    void Update()
    {
        
    }
    public Vector3 GetEnemie(Vector3 pos)
    {
        //Clear Nulls & far enemies
        UtilsNagu.RemoveAllNulls(ref enemies);
        UtilsNagu.RemoveAllInactive(ref enemies);
        RemoveByDistance();
        RemoveEnemiesDead();

        Vector3 position =  Vector3.zero;
        float distance = 100;
        for (int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] != null)
            {
                if (Vector3.Distance(enemies[i].transform.position, pos) < distance)
                {
                    distance = Vector3.Distance(enemies[i].transform.position, pos);
                    position = new Vector3(enemies[i].transform.position.x, pos.y, enemies[i].transform.position.z);
                }
            }
        }

        return position;
    }
    public Vector3 GetEnemiePos(Vector3 pos)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null || enemies[i].tag != "Enemy" || !enemies[i].activeSelf)
            {
                enemies.RemoveAt(i);
            }
            if (enemies[i].transform.parent != null)
            {
                if (!enemies[i].transform.parent.gameObject.activeSelf)
                {
                    enemies.RemoveAt(i);

                }
            }

        }
        Vector3 position = Vector3.zero;
        float distance = 100;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position, pos) < distance)
            {
                distance = Vector3.Distance(enemies[i].transform.position, pos);
                position = new Vector3(enemies[i].transform.position.x, enemies[i].transform.position.y, enemies[i].transform.position.z);
            }

        }



        return position;
    }

    public void RemoveEnemiesDead()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetComponent<EnemyBase>().IsDead)
            {
                enemies.RemoveAt(i);
            }
        }
    }

    public GameObject GetEnemieGameObject(Vector3 pos)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null || enemies[i].tag != "Enemy" || !enemies[i].activeSelf)
            {
                enemies.RemoveAt(i);
            }
            if (enemies[i].transform.parent != null)
            {
                if (!enemies[i].transform.parent.gameObject.activeSelf)
                {
                    enemies.RemoveAt(i);

                }
            }
        }
        GameObject enemy = null;
        float distance = 100;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position, pos) < distance)
            {
                distance = Vector3.Distance(enemies[i].transform.position, pos);
                enemy = enemies[i];
            }

        }



        return enemy;
    }

    public void RemoveByDistance()
    { 
        List<GameObject> enemiesToRemove = new List<GameObject>();
        foreach (var enemy in enemies)
        {
            if(enemy != null)
            {
                if (Vector3.Distance(GameManager.Instance.Player.transform.position, enemy.transform.position) > 15)
                {
                    enemiesToRemove.Add(enemy);
                }
            }
        }
        foreach (var enemyToRemove in enemiesToRemove)
        {
            enemies.Remove(enemyToRemove);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!enemies.Contains(other.gameObject))
            {
                enemies.Add(other.gameObject);
            }
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        if (!enemies.Contains(other.gameObject))
    //        {
    //            enemies.Add(other.gameObject);
    //        }
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (enemies.Contains(other.gameObject))
            {
                enemies.Remove(other.gameObject);
            }

        }
    }

    public GameObject GetEnemyDebil(Vector3 pos)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null || enemies[i].tag != "Enemy")
            {
                enemies.RemoveAt(i);
            }
        }
        GameObject position = null;
        for (int i = 0; i < enemies.Count; i++)
        {
            //if (enemies[i].GetComponent<EnemySkeletonSword>().debil == true)
            //{
            //    position = enemies[i];
            //}

        }



        return position;
    }

}
