using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth;

    private int currentHealth;

    [SerializeField]
    private DamageText damageTextPrefab;

    public DamageText DamageText
    {
        get
        {
            return damageTextPrefab;
        }
    }

    public event IDamageable.TakeDamageEvent onTakeDamage;
    public event IDamageable.DeathEvent onDeath;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public int MaxHealth
    {
        get
        { 
            return maxHealth; 
        }
    }

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(int damage, Vector3 position)
    {
        currentHealth -= damage;

        onTakeDamage?.Invoke(damage, position);

        int dmg = Convert.ToInt32(damage);
        DamageText damageText = Instantiate(damageTextPrefab, position, Quaternion.identity);
        damageText.damage = dmg;

        if (currentHealth <= 0)
        {
            onDeath?.Invoke(transform.position);
            Death();
        }
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
