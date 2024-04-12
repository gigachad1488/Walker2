using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TeleportAbility : MonoBehaviour, IAbility
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
    [SerializeField]
    private LayerMask layers;
    [SerializeField]
    private GameObject teleportIndicatorParticlesPrefab;
    private GameObject teleportIndicatorParticles;

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
        buffParticles.Stop();
        ParticlesVisible(false);
    }

    public void Fire()
    {
        buffParticles.Play();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.Move(teleportIndicatorParticles.transform.position, rb.rotation);
        Destroy(teleportIndicatorParticles);
        teleportIndicatorParticles = null;
        ParticlesVisible(false);
        //Invoke(nameof(DisableBuff), duration);
    }

    public void ShowIndicator()
    {       
        if (teleportIndicatorParticles == null)
        {
            teleportIndicatorParticles = Instantiate(teleportIndicatorParticlesPrefab, transform.position, Quaternion.identity);
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        ParticlesVisible(true);

        if (Physics.Raycast(ray, out hit, value, layers))
        {
            teleportIndicatorParticles.transform.position = hit.point + (hit.normal * 0.2f);
        }
        else
        {
            teleportIndicatorParticles.transform.position = ray.GetPoint(value);
        }
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
