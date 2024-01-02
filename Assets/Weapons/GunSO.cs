using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon Config/Gun", order = 0)]
public class GunSO : ScriptableObject
{
    public GunType type;
    public string name;

    public int maxAmmo;
    public int currentAmmo;

    public GameObject modelPrefab;
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;
    public DamageConfigurationSO damageConfig;
    [SerializeField]
    private ShootConfigurationSO shootConfigSO;
    [HideInInspector]
    public ShootConfigurationSO shootConfig;

    [SerializeField]
    private TrailConfigurationSO trailConfigSO;
    [HideInInspector]
    public TrailConfigurationSO trailConfig;

    [SerializeField]
    private AudioConfigSO audioConfigSO;
    [HideInInspector]
    public AudioConfigSO audioConfig;

    public DamageText damageTextPrefab;

    public GameObject hitPrefab;
    private ObjectPool<GameObject> hitPool;

    private MonoBehaviour activeMB;
    public GameObject model;
    private float lastShootTime;
    private AudioSource shootingAudioSource;
    private ParticleSystem shootSystem;
    private ObjectPool<TrailRenderer> trailPool;
    private ProceduralRecoil recoil;
    private PlayerAction playerAction;

    public void Spawn(Transform parent, MonoBehaviour activemb)
    {
        shootConfig = Instantiate(shootConfigSO);
        audioConfig = Instantiate(audioConfigSO);
        trailConfig = Instantiate(trailConfigSO);
        activeMB = activemb;
        lastShootTime = 0;
        trailPool = new ObjectPool<TrailRenderer>(CreateTrail);
        hitPool = new ObjectPool<GameObject>(CreateHit);
        model = Instantiate(modelPrefab);
        model.transform.SetParent(parent, false);
        model.transform.localPosition = spawnPoint;
        model.transform.localRotation = Quaternion.Euler(spawnRotation);

        shootSystem = model.GetComponentInChildren<ParticleSystem>();
        recoil = model.GetComponentInParent<ProceduralRecoil>();
        shootingAudioSource = model.GetComponentInChildren<AudioSource>();
        audioConfig.audioSource = shootingAudioSource;

        currentAmmo = maxAmmo;
    }

    public void Shoot()
    {
        shootSystem.Play();
        audioConfig.PlayShootingCLip();

        //Vector3 spreadAmount = shootConfig.GetSpread(0);
        Vector3 shootDirection = model.transform.parent.forward;

        if (Physics.Raycast(shootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, shootConfig.hitMask))
        {
            if (hit.point != null && hit.collider != null)
            {
                activeMB.StartCoroutine(PlayHit(hit));

                if (hit.transform.TryGetComponent<HitBox>(out HitBox hitbox))
                {
                    hitbox.OnHit(damageConfig.GetDamage(), damageConfig.force, hit.point, shootDirection);
                }
                else if (hit.transform.TryGetComponent<IDamageable>(out IDamageable damageable)) 
                {
                    damageable.Damage(damageConfig.GetDamage(), damageConfig.force, hit.point, shootDirection);
                }
            }
            activeMB.StartCoroutine(PlayTrail(shootSystem.transform.position, hit.point, hit));
        }
        else
        {
            activeMB.StartCoroutine(PlayTrail(shootSystem.transform.position, shootSystem.transform.position + (shootDirection * trailConfig.missDistance), new RaycastHit()));
        }
    }

    private IEnumerator PlayHit(RaycastHit hit)
    {
        GameObject ht = hitPool.Get();
        ht.SetActive(true);
        ht.transform.position = hit.point + (hit.normal * 0.1f);
        yield return new WaitForSeconds(0.2f);
        ht.SetActive(false);
        hitPool.Release(ht);
    }

    private IEnumerator PlayTrail(Vector3 startPoint, Vector3 endPoint, RaycastHit hit)
    {
        TrailRenderer instance = trailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = startPoint;

        yield return null;

        instance.emitting = true;
        float distance = Vector3.Distance(startPoint, endPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(startPoint, endPoint, Mathf.Clamp01(1 - (remainingDistance / distance)));

            remainingDistance -= trailConfig.simulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = endPoint;

        yield return new WaitForSeconds(trailConfig.duration);
        yield return null;

        instance.emitting = false;
        instance.gameObject.SetActive(false);
        trailPool.Release(instance);

        /*
        if (hit.point != null && hit.collider != null)
        {
            activeMB.StartCoroutine(PlayHit(hit));

            if (hit.transform.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                //damageable.Damage(damageConfig.GetDamage(), damageConfig.force, hit.point);
            }
        }
        */
    }

    private GameObject CreateHit()
    {
        GameObject hit = Instantiate(hitPrefab);
        return hit;
    }

    private TrailRenderer CreateTrail()
    {
        GameObject instance = new GameObject("Bullet Trail");
        TrailRenderer trail = instance.AddComponent<TrailRenderer>();
        trail.colorGradient = trailConfig.color;
        trail.material = trailConfig.material;
        trail.widthCurve = trailConfig.widthCurve;
        trail.time = trailConfig.duration;
        trail.minVertexDistance = trailConfig.minVertexDistance;
        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        return trail;
    }
}
