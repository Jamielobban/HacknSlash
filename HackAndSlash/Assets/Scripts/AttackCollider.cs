using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using DamageNumbersPro;

public class AttackCollider : MonoBehaviour
{


    public MMFeedbacks enemyHitFeedback;
    public float Knockback;
    public float KnockbackY;


    // *** NO Need ***//
    public PlayerControl.HealthState healthState;
    public PlayerControl.PassiveCombo attack;
    [SerializeField]
    public float spawnDelay;
    public float spawnDistance;
    public float upDistance;
    public bool enemyStandUp;
    public string enemyHitAnim;

    //*** LINE 555 PLAYER CONTROL setter ***//
    // *** Need 100% *** //
    public float damage;
    public bool isCrit;
    public bool toAir = false;

    void Start()
    {

    }
    public void SetFeedback(MMFeedbacks enemyHitFeedback)
    {
        this.enemyHitFeedback = enemyHitFeedback;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            Vector3 collisionPoint = FindClosestPointOnCollider(other, transform.position);
            IDamageable enemy = other.GetComponent<IDamageable>();
            enemy.TakeDamage(damage, isCrit, collisionPoint);
            if(toAir)
            {
                enemy.AirDamageable();
            }
        }
    }

    Vector3 FindClosestPointOnCollider(Collider collider, Vector3 point) => collider.ClosestPointOnBounds(point);
}
