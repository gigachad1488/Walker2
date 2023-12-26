using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private GameObject abilityParticlesPrefab;
    public GameObject AbilityParticlesPrefab
    {
        get
        {
            return abilityParticlesPrefab;
        }
    }

    private GameObject abilityParticlesGM;
    private ParticleSystem abilityParticles;

    [SerializeField]
    private ParticleSystem buffParticles;

    [SerializeField]
    private Sprite abilitySprite;

    public Sprite AbilitySprite
    {
        get
        {
            return abilitySprite;
        }
    }

    public void SetPlayer(PlayerController player, Transform abilityHand, Canvas uiCanvas)
    {
        this.player = player;
        abilityParticlesGM = Instantiate(AbilityParticlesPrefab, abilityHand);
        abilityParticlesGM.transform.localPosition = new Vector3(-0.065f, 0.088f, 0);
        abilityParticles = abilityParticlesGM.GetComponent<ParticleSystem>();
        buffParticles = Instantiate(buffParticles, uiCanvas.transform);
        ParticlesVisible(false);
    }

    public void Fire()
    {
        buffParticles.Play();
        StaticData.msBuffMult += value * 0.8f;
        StaticData.jumpBuffMult += value * 0.3f;
        StaticData.dmgBuffMult += value;
        ParticlesVisible(false);
        Invoke(nameof(DisableBuff), duration);
    }

    public void ShowIndicator()
    {
        ParticlesVisible(true);
    }

    private void DisableBuff()
    {
        StaticData.msBuffMult -= value;
        StaticData.msBuffMult -= value;
        StaticData.jumpBuffMult -= value;
        StaticData.dmgBuffMult -= value;
        buffParticles.Stop();
    }

    public void ParticlesVisible(bool visible)
    {
        if (visible)
        {
            abilityParticlesGM.SetActive(true);
            abilityParticles.Play();
        }
        else
        {
            abilityParticles.Stop();
            abilityParticlesGM.SetActive(false);         
        }
    }

    public void SetUi(Image ai)
    {
        ai.sprite = abilitySprite;
    }
}
