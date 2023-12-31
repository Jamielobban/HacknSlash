using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEnemies : MonoBehaviour
{
    public List<GameObject> enemies;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector3 GetEnemie(Vector3 pos)
    {
        for(int i = 0; i < enemies.Count;i++)
        {
            if(enemies[i] == null || enemies[i].tag != "Enemy")
            {
                enemies.RemoveAt(i);
            }
        }
        Vector3 position =  Vector3.zero;
        float distance = 100;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (Vector3.Distance(enemies[i].transform.position,pos) < distance)
            {
                distance = Vector3.Distance(enemies[i].transform.position, pos);
                position = new Vector3(enemies[i].transform.position.x, pos.y, enemies[i].transform.position.z);
            }

        }



        return position;
    }
    public Vector3 GetEnemiePos(Vector3 pos)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null || enemies[i].tag != "Enemy")
            {
                enemies.RemoveAt(i);
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

    public GameObject GetEnemieGameObject(Vector3 pos)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null || enemies[i].tag != "Enemy")
            {
                enemies.RemoveAt(i);
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(!enemies.Contains(other.gameObject))
            {
                enemies.Add(other.gameObject);
            }
        }
    }
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
        float distance = 100;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetComponent<EnemySkeletonSword>().debil == true)
            {
                position = enemies[i];
            }

        }



        return position;
    }

}
