using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Walker2.Controller;

public class BuffAbility : MonoBehaviour, IAbility
{
    private PlayerController player;

    public float value;
    public float duration;
    public float cd;

    private bool canFire = false;

    private ParticleSystem buffParticles;

    private void Start()
    {
        buffParticles = GetComponentInChildren<ParticleSystem>();
        canFire = true;
    }

    public void SetPlayer(PlayerController player)
    {
        this.player = player;
    }

    public void Fire()
    {
        if (canFire) 
        {
            Debug.Log("FRE");
            canFire = false;
            StartCoroutine(Cooldown());
            buffParticles.Play();
            StaticData.msBuffMult += value;
            StaticData.msBuffMult += value;
            StaticData.jumpBuffMult += value;
            StaticData.dmgBuffMult += value;
            StartCoroutine(DisableBuff());
        }
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

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cd);
        canFire = true;
    }
}
