using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int mult = 1;

    public EnemyHealth health;

    public void OnHit(int damage, float force, Vector3 point, Vector3 direction)
    {
        health.Damage(damage * mult, force, point, direction);
    }

    public void OnHit(int damage, float force, Vector3 point, Vector3 direction, int mult)
    {
        health.Damage(damage * mult, force, point, direction);
    }
}
