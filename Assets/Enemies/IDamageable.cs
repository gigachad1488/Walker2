using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public int CurrentHealth { get; }
    public int MaxHealth { get; }

    public delegate void TakeDamageEvent(int damage, Vector3 position);
    public event TakeDamageEvent onTakeDamage;

    public delegate void DeathEvent(Vector3 position);
    public event DeathEvent onDeath;

    public void Damage(int damage, float force, Vector3 position, Vector3 direction);

    public DamageText DamageText { get; }
}
