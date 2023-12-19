using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Walker2.Controller;

public class BuffAbility : MonoBehaviour, IAbility
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
    private GameObject abilityParticles;
    public GameObject AbilityParticles
    {
        get
        {
            return abilityParticles;
        }
    }

    private ParticleSystem buffParticles;

    private void Start()
    {
        buffParticles = GetComponentInChildren<ParticleSystem>();
    }

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }

    public void Fire()
    {
        buffParticles.Play();
        StaticData.msBuffMult += value;
        StaticData.msBuffMult += value;
        StaticData.jumpBuffMult += value;
        StaticData.dmgBuffMult += value;
        StartCoroutine(DisableBuff());
    }

    private IEnumerator DisableBuff()
    {
        yield return new WaitForSeconds(duration);
        StaticData.msBuffMult -= value;
        StaticData.msBuffMult -= value;
        StaticData.jumpBuffMult -= value;
        StaticData.dmgBuffMult -= value;
        buffParticles.Stop();
    }
}
