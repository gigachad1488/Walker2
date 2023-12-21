using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballUnit : MonoBehaviour
{
    public Vector3 direction;
    public float damage = 0;
    public float speed = 0;
    public float radius = 0;
    public float maxFlightTime = 0;

    [SerializeField]
    private ParticleSystem explodeParticles;

    private Rigidbody rb;

    private void Start()
    {    
        if (speed > 0)
        {
            rb = GetComponent<Rigidbody>();
            GetComponent<Collider>().enabled = true;
            rb.constraints = RigidbodyConstraints.None;
            var m = GetComponent<ParticleSystem>().main;
            m.prewarm = true;
            var mm = explodeParticles.main;
            mm.startSize = radius;
            rb.velocity = direction * speed;
            Invoke(nameof(Explode), maxFlightTime);
        }
    }

    private void Explode()
    {
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        explodeParticles.gameObject.SetActive(true);
        explodeParticles.Play();

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider item in hits) 
        {
            if (item.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                damagable.Damage(damage);
            }
        }

        Invoke(nameof(Destroy), 0.8f);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode(); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
