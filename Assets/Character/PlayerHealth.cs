using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthBar))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth;

    private int currentHealth;
    [HideInInspector]
    public HealthBar healthBar;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    public DamageText DamageText => throw new System.NotImplementedException();

    public event IDamageable.TakeDamageEvent onTakeDamage;
    public event IDamageable.DeathEvent onDeath;

    private void Start()
    {
        healthBar = GetComponent<HealthBar>();
        currentHealth = maxHealth;
        healthBar.Set(1, 1);
    }

    public void Damage(int damage, float force, Vector3 position, Vector3 direction)
    {
        
        currentHealth -= damage;
        Debug.Log(currentHealth);
        healthBar.Change((float)currentHealth / (float)maxHealth);
    }
}
