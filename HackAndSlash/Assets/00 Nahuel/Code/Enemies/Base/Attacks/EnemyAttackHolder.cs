using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHolder : MonoBehaviour
{
    public List<EnemyBaseAttack> attacks = new List<EnemyBaseAttack>();
    public void UseAbility(int attack)
    {
        attacks[attack].Use();
    }
}
