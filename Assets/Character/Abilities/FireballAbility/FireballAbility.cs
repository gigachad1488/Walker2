using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Walker2.Controller;

public class FireballAbility : MonoBehaviour, IAbility
{
    private PlayerController player;

    [SerializeField]
    private int value;
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

    private FireballUnit activeFireball;
    private Transform abilityHand;

    public void SetPlayer(PlayerController player, Transform abilityHand, Canvas uiCanvas)
    {
        this.player = player;
        this.abilityHand = abilityHand;       
    }

    public void Fire()
    {
        Vector3 dir;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, layers))
        {
            dir = hit.point;
        }
        else
        {
            dir = ray.GetPoint(100);
        }
        activeFireball.transform.SetParent(null);
        activeFireball.Fire((dir - activeFireball.transform.position).normalized);
        activeFireball = null;
        //Invoke(nameof(DisableBuff), duration);
    }

    public void ShowIndicator()
    {
        if (activeFireball == null)
        {
            activeFireball = Instantiate(AbilityParticlesPrefab, abilityHand).GetComponent<FireballUnit>();
            activeFireball.transform.localPosition = new Vector3(-0.155f, 0.12f, 0);
            activeFireball.damage = value;
            activeFireball.radius = radius;
            activeFireball.maxFlightTime = duration;
            activeFireball.speed = speed;       
        }    
    }
}
