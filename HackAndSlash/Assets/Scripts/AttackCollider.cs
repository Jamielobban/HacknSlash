using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DamageNumbersPro;


public class AttackCollider : MonoBehaviour
{
    PlayerControl _player;
    public PlayerControl.HitState state;
    public GameObject[] hitEffect;

    public List<GameObject> targets;
    void Start()
    {
        _player = FindObjectOfType<PlayerControl>();
    }
    private IEnumerator ReturnEffect(float time, GameObject slash, GameObject parent)
    {
        yield return new WaitForSecondsRealtime(time);


            slash.transform.parent = parent.transform;
            slash.transform.localPosition = Vector3.zero;
            slash.transform.localEulerAngles = Vector3.zero;
            slash.SetActive(false);
        

    }

    float CalculateDamage(Transform enemyPos)
    {
        if(targets.Contains(enemyPos.gameObject))
        {
            return 0;
        }
        float damage = (int)state * _player.attackDamage;

        int rand = Random.Range(0, _player.stats.maxCritChance);
        if (rand < _player.critChance)
        {
            damage *= _player.critDamageMultiplier;
        }

        //if(_player.currentComboAttacks.combo == PlayerControl.ComboAtaques.air1)
        //{
        Vector3 pos = _player.gameObject.transform.position;
        pos.y = enemyPos.position.y;
        Vector3 dir = pos - enemyPos.position;
        dir = dir.normalized;
        dir *= 2;



        for (int i = 0; i < hitEffect.Length; i++)
        {
            hitEffect[i].transform.position = enemyPos.position + dir;
            if (_player.currentComboAttacks.combo == PlayerControl.ComboAtaques.air1)
            {
                hitEffect[i].transform.position += new Vector3(0, ((_player.gameObject.transform.position.y - enemyPos.position.y)/4)*3, 0);
            }
            else
            {
                hitEffect[i].transform.position += new Vector3(0, 1, 0);

            }
            StartCoroutine(ReturnEffect(0.5f, hitEffect[i].transform.GetChild(0).gameObject, hitEffect[i]));
            hitEffect[i].transform.GetChild(0).gameObject.SetActive(true);
            hitEffect[i].transform.GetChild(0).parent = GameObject.FindGameObjectWithTag("Slashes").transform;
        }





        //}
        //else
        //{
        //    Vector3 pos = enemyPos.gameObject.transform.position;
        //    pos.y = _player.gameObject.transform.position.y;
        //    Vector3 dir = pos - _player.gameObject.transform.position;
        //    dir = dir.normalized;
        //    dir *= 3;

        //    hitEffect.transform.position = pos + dir;

        //    hitEffect.transform.GetChild(0).gameObject.SetActive(true);
        //    StartCoroutine(ReturnEffect(1, hitEffect.transform.GetChild(0).gameObject));
        //    hitEffect.transform.GetChild(0).parent = GameObject.FindGameObjectWithTag("Slashes").transform;
        //}
        targets.Add(enemyPos.gameObject);

        Invoke("ClearList", 0.2f);
        return damage;
    }
    void ClearList()
    {
        targets.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {

        other.GetComponent<IDamageableEnemy>()?.TakeDamage(state, CalculateDamage(other.gameObject.transform));
    }

}
