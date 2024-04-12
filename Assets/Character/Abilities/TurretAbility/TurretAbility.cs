using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretAbility : MonoBehaviour, IAbility
{
    private PlayerController player;

    [SerializeField]
    private TurretUnit turretUnitPrefab;

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
    private float range = 5f;

    private GameObject abilityParticlesGM;

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
    private Sprite abilitySprite;

    private TurretUnit activeTurret;

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
    }

    public void Fire()
    {
        activeTurret.Deploy(duration, Convert.ToInt32(value));
        activeTurret = null;

        abilityParticlesGM.SetActive(false);
    }

    public void ShowIndicator()
    {
        abilityParticlesGM.SetActive(true);

        if (activeTurret == null) 
        {
            activeTurret = Instantiate(turretUnitPrefab);
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range, layers))
        {
            activeTurret.transform.position = hit.point + (hit.normal * 0.3f);
        }
        else
        {        
            Vector3 point = ray.GetPoint(range);

            if (Physics.Raycast(point, Vector3.down, out RaycastHit groundHit, float.MaxValue, layers))
            {
                activeTurret.transform.position = groundHit.point + (groundHit.normal * 0.3f);
            }
            else
            {
                activeTurret.transform.position = point;
            }
        }
    }


    public void SetUi(Image ai)
    {
        ai.sprite = abilitySprite;
    }
}
