using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Walker2.Controller;

public class FireballAbility : MonoBehaviour, IAbility
{
    private PlayerController player;

    [SerializeField]
    private float value;
    public float Value
    {
        get
        {
            return duration;
        }
    }

    [SerializeField]
    private float duration;
    public float Duration
    {
        get
        {
            return duration;
        }
    }

    [SerializeField]
    private float cd;
    public float Cd
    {
        get
        {
            return cd;
        }
    }

    [SerializeField]
    private GameObject abilityParticlesPrefab;
    public GameObject AbilityParticlesPrefab
    {
        get
        {
            return abilityParticlesPrefab;
        }
    }

    public float radius = 3;
    public float speed = 2;

    [SerializeField]
    private LayerMask layers;

    private GameObject abilityParticlesGM;
    private ParticleSystem abilityParticles;

    public void SetPlayer(PlayerController player, Transform abilityHand, Canvas uiCanvas)
    {
        this.player = player;
        abilityParticlesGM = Instantiate(AbilityParticlesPrefab, abilityHand);
        abilityParticlesGM.transform.localPosition = new Vector3(-0.155f, 0.12f, 0);
        abilityParticles = abilityParticlesGM.GetComponent<ParticleSystem>();
        ParticlesVisible(false);
    }

    public void Fire()
    {
        Vector3 dir;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        ParticlesVisible(false);
        if (Physics.Raycast(ray, out hit, 100, layers))
        {
            dir = hit.point;
        }
        else
        {
            dir = ray.GetPoint(100);
        }

        FireballUnit fireball = Instantiate(AbilityParticlesPrefab, abilityParticlesGM.transform.position, Quaternion.identity).GetComponent<FireballUnit>();
        fireball.damage = value;
        fireball.direction = (dir - abilityParticlesGM.transform.position).normalized;
        fireball.radius = radius;
        fireball.maxFlightTime = duration;
        fireball.speed = speed;
        //Invoke(nameof(DisableBuff), duration);
    }

    public void ShowIndicator()
    {
        ParticlesVisible(true);
    }

    public void ParticlesVisible(bool visible)
    {
        if (visible)
        {
            abilityParticles.gameObject.SetActive(true);
            //abilityParticles.Play();
        }
        else
        {
            //abilityParticles.Stop();
            abilityParticles.gameObject.SetActive(false);
        }
    }
}
