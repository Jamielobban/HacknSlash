using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerControl;
using MoreMountains.Feedbacks;

[CreateAssetMenu(fileName = "New Combo", menuName = "Combo", order = 1)]
public class Blessing : ScriptableObject
{
    public string giveName;
    public List<PlayerControl.PassiveCombo> passiveCombo = new List<PlayerControl.PassiveCombo>();
    public GameObject visualEffect;
    public PlayerControl.HealthState healthState;
    public MMFeedbacks enemyFeedback;

    public float damage;
    public Vector2 knockback;
    public string hit;
    public bool EnemyStandUp;
    
    //public void spawnEffect(Vector3 pos)
    //{
    //    Instantiate(visualEffect,pos,Quaternion.identity);
    //}

}
