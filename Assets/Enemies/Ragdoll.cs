using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class Ragdoll : MonoBehaviour
{
    private IDamageable damageable;

    private Rigidbody[] rigidbodies;

    private Animator animator;

    void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<IDamageable>();

        EnemyHealth enemyHealth = GetComponent<EnemyHealth>();

        foreach (Rigidbody item in rigidbodies) 
        {
            if (item.gameObject.name.Contains("Head", System.StringComparison.OrdinalIgnoreCase))
            {
                HitBox hitbox = item.AddComponent<HitBox>();
                hitbox.health = enemyHealth;
                hitbox.mult = 2;
                
            }
            else
            {
                HitBox hitbox = item.AddComponent<HitBox>();
                hitbox.health = enemyHealth;
            }
        }

        damageable.onDeath += ActivateRagdoll;
    }

    private void OnEnable()
    {
        DiactivateRagdoll();
    }

    public void DiactivateRagdoll()
    {
        foreach (Rigidbody item in rigidbodies) 
        {
            item.isKinematic = true;
        }

        animator.enabled = true;
    }

    public void ActivateRagdoll(Vector3 pos)
    {
        foreach (Rigidbody item in rigidbodies)
        {
            item.isKinematic = false;
        }

        animator.enabled = false;
    }
}
