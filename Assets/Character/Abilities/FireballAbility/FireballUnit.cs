using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballUnit : MonoBehaviour
{
    public int damage = 10;
    public float speed = 10;
    public float radius = 2;
    public float maxFlightTime = 4;
    public float force;

    [SerializeField]
    private ParticleSystem explodeParticles;

    private Rigidbody rb;

    public void Fire(Vector3 direction)
    {
        rb = GetComponent<Rigidbody>();
        GetComponent<Collider>().enabled = true;
        rb.constraints = RigidbodyConstraints.None;
        var mm = explodeParticles.main;
        mm.startSize = radius * 0.5f;
        rb.velocity = direction * speed;
        Invoke(nameof(Explode), maxFlightTime);
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
            if (item.TryGetComponent<HitBox>(out HitBox hitbox))
            {
                hitbox.OnHit(damage, force, item.transform.position, item.transform.position - transform.position, 1);
                Invoke(nameof(Destroy), 0.8f);
                return;
            }
        }

        Invoke(nameof(Destroy), 0.8f);
    }

    public void Destroy()
    {       
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }
}
