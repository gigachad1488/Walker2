using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int mult = 1;

    public EnemyHealth health;

    public void OnHit(int damage, Vector3 point)
    {
        health.Damage(damage * mult, point);
    }
}
