using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Walker2.Controller;

public class AbilityManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private GameObject abilityPrefab;

    [SerializeField]
    private Transform abilityHand;

    private GameObject abilityParticleGM;
    private ParticleSystem abilityParticle;

    private bool canFire = true;
    
    private IAbility ability;

    public float cd;

    private void Start()
    {
        GameObject abl = Instantiate(abilityPrefab, transform);
        SetAbility(abl.GetComponent<IAbility>());
        canFire = true;
    }

    public void SetAbility(IAbility ability)
    {
        ability.SetPlayer(player);
        this.ability = ability;
        cd = ability.Cd;
        abilityParticleGM = Instantiate(ability.AbilityParticles, abilityHand);
        abilityParticleGM.transform.localPosition = new Vector3(-0.065f, 0.088f, 0);
        abilityParticle = abilityParticleGM.GetComponent<ParticleSystem>();
        ParticlesVisible(false);
    }

    public void FireAbility()
    {
        if (canFire)
        {
            ability.Fire();
        }
    }

    public void ParticlesVisible(bool visible)
    {
        if (visible)
        {
            abilityParticleGM.SetActive(true);
            abilityParticle.Play();
        }
        else
        {
            abilityParticleGM.SetActive(false);
            abilityParticle.Stop();
        }
    }
}
