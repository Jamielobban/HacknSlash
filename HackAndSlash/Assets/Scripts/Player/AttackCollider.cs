using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DamageNumbersPro;

public class AttackCollider : MonoBehaviour
{
    PlayerControl _player;
    public GameObject[] hitEffect;
    public GameObject critHitEffect;
    public MMFeedbacks critHitFeedback;
    public MMFeedbacks hitFeedback;

    public List<GameObject> targets;

    public float baseDamage;

    public PlayerControl.Ataques attack;

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

    (float, DamageNumber) CalculateDamage (Transform enemyPos)
    {
        if(targets.Contains(enemyPos.gameObject))
        {
            return (0, _player.basicDamageHit);
        }
        bool isCrit;
        float damage = _player.attackBoost * _player.multiplierCombos * baseDamage * _player.attackDamage + Random.Range(0.5f, 1.5f);

        int rand = Random.Range(0, _player.stats.maxCritChance);

        _player.OnHit?.Invoke();

        Vector3 pos = _player.gameObject.transform.position;
        pos.y = enemyPos.position.y;
        Vector3 dir = pos - enemyPos.position;
        dir = dir.normalized;
        dir *= 0.5f;

        if (rand < _player.critChance)
        {
            damage *= _player.critDamageMultiplier;
            critHitEffect.transform.position = enemyPos.position + dir;
            if (_player.currentComboAttacks.combo == PlayerControl.ComboAtaques.air1)
            {
                critHitEffect.transform.position += new Vector3(0, ((_player.gameObject.transform.position.y - enemyPos.position.y) / 4) * 3, 0);
            }
            else
            {
                critHitEffect.transform.position += new Vector3(0, 1, 0);

            }
            StartCoroutine(ReturnEffect(0.5f, critHitEffect.transform.GetChild(0).gameObject, critHitEffect));
            critHitEffect.transform.GetChild(0).gameObject.SetActive(true);
            critHitEffect.transform.GetChild(0).parent = GameObject.FindGameObjectWithTag("Slashes").transform;
            critHitFeedback.PlayFeedbacks();
            isCrit = true;
        }
        else
        {
            for (int i = 0; i < hitEffect.Length; i++)
            {
                hitEffect[i].transform.position = enemyPos.position + dir;
                if (_player.currentComboAttacks.combo == PlayerControl.ComboAtaques.air1)
                {
                    hitEffect[i].transform.position += new Vector3(0, ((_player.gameObject.transform.position.y - enemyPos.position.y) / 4) * 3, 0);
                }
                else
                {
                    hitEffect[i].transform.position += new Vector3(0, 1, 0);

                }
                StartCoroutine(ReturnEffect(0.5f, hitEffect[i].transform.GetChild(0).gameObject, hitEffect[i]));
                hitEffect[i].transform.GetChild(0).gameObject.SetActive(true);
                hitEffect[i].transform.GetChild(0).parent = GameObject.FindGameObjectWithTag("Slashes").transform;
                hitFeedback.PlayFeedbacks();

            }
            isCrit = false;

        }
        
        targets.Add(enemyPos.gameObject);

        Invoke("ClearList", 0.2f);
        return (damage, isCrit ? _player.criticalDamageHit : _player.basicDamageHit);
    }
    void ClearList()
    {
        targets.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IDamageable>() != null)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            (float damageValue, DamageNumber damageNumber) = CalculateDamage(other.gameObject.transform);

            damageable.TakeDamage(damageValue, damageNumber);

            LevelManager.Instance.ScoreManager.AddEnemyScore(1.8f);

            if (attack.canBurn)
            {
                damageable.ApplyBurn();
            }
            if(attack.canStun)
            {
                //damageable.ApplyStun(attack.timeStun);
            }

        }


    }

}
