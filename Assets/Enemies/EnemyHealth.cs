using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int maxHealth;

    private int currentHealth;

    [SerializeField]
    private DamageText damageTextPrefab;

    //private EnemyMovement movement = null;

    public bool dead = false;

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

        if (dead)
        {
            //movement.enabled = true;
            //movement.agent.isStopped = false;
        }

        dead = false;
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

        //movement = GetComponent<EnemyMovement>();
    }

    public void Damage(int damage, float force, Vector3 position, Vector3 direction)
    {
        if (!dead)
        {
            currentHealth -= damage;
            onTakeDamage?.Invoke(damage, position);

            int dmg = Convert.ToInt32(damage);
            DamageText damageText = Instantiate(damageTextPrefab, position, Quaternion.identity);
            damageText.damage = dmg;

            if (currentHealth <= 0)
            {
                onDeath?.Invoke(position);
                Death((direction).normalized, force);
            }
        }

    }

    public void Death(Vector3 direction, float force)
    {
        dead = true;
        //movement.agent.isStopped = true;
        //movement.enabled = false;
        Invoke(nameof(DisableEnemy), 10);

        GetComponent<NavMeshAgent>().isStopped = true;

        Rigidbody rb = GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    public void DisableEnemy()
    {
        Destroy(gameObject);
    }
}
