using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IDamageable))]
public class ParticlesOnDeath : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particles;

    public IDamageable damageable;

    private void Awake()
    {
        damageable = GetComponent<IDamageable>();
    }

    private void OnEnable()
    {
        damageable.onDeath += OnDeath;
    }

    private void OnDeath(Vector3 position)
    {
        Instantiate(particles, position, Quaternion.identity);     
    }
}
