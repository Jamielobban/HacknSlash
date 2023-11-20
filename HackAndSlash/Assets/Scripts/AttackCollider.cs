using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class AttackCollider : MonoBehaviour
{

    public MMFeedbacks enemyHitFeedback;
    public float Knockback;
    public float KnockbackY;
    public bool enemyStandUp;
    public string enemyHitAnim;
    public float damage;

    public PlayerControl.HealthState healthState;

    [SerializeField]
    public float spawnDelay;
    public float spawnDistance;
    public float upDistance;

    public bool isCrit;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void SetFeedback(MMFeedbacks enemyHitFeedback)
    {
        this.enemyHitFeedback = enemyHitFeedback;
    }
    //public float GetCritOrDamage()
    //{
    //    float baseDamage = this.transform.GetComponentInParent<PlayerControl>().attackDamage;

    //    bool isCrit = IsCriticalHit();

    //    float finalDamage = CalculateDamage(baseDamage, isCrit);
    //   // Debug.Log("Final Damage: " + finalDamage);

    //    return finalDamage;
    //}

    //private bool IsCriticalHit()
    //{
    //    float randomValue = Random.Range(0f, 100f);  
    //    isCrit = randomValue <= this.transform.GetComponentInParent<PlayerControl>().critChance;   

       
    //    Debug.Log("Is Critical Hit: " + isCrit);

    //    return isCrit;
    //}

    //private float CalculateDamage(float baseDamage, bool isCrit)
    //{
    //    float finalDamage = isCrit ? baseDamage * (2f * this.transform.GetComponentInParent<PlayerControl>().critDamageMultiplier) : baseDamage;

    //    //Debug.Log("Final Damage: " + finalDamage);

    //    return finalDamage;
    //}
}
