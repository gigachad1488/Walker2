using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public float maxHealth;
    public float currentHealth;

    [SerializeField]
    private DamageText damageTextPrefab;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) 
        {
            currentHealth = maxHealth;           
        }
    }
}
