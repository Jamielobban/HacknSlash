using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, bool isCrit, Vector3 collisionPoint);
    void Heal(float amount);
    void AirDamageable();
}

