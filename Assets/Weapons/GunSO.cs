using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon Config/Gun", order = 0)]
public class GunSO : ScriptableObject
{
    public GunType type;
    public string name;
    public float damage;
    public int ammo;
    public GameObject modelPrefab;
    public Vector3 spawnPoint;
    public Vector3 spawnRotation;
    public ShootConfigurationSO shootConfig;
    public TrailConfigurationSO trailConfig;
    public DamageText damageTextPrefab;

    public GameObject hitPrefab;
    private ObjectPool<GameObject> hitPool;

    private MonoBehaviour activeMB;
    public GameObject model;
    private float lastShootTime;
    private ParticleSystem shootSystem;
    private ObjectPool<TrailRenderer> trailPool;
    private ProceduralRecoil recoil;
    private PlayerAction playerAction;

    public void Spawn(Transform parent, MonoBehaviour activemb)
    {
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
    }

    public void Shoot()
    {
        shootSystem.Play();

        //Vector3 spreadAmount = shootConfig.GetSpread(0);
        Vector3 shootDirection = model.transform.parent.forward;

        if (Physics.Raycast(shootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, shootConfig.hitMask))
        {
            //GameObject hp = Instantiate(hitPrefab, hit.point, Quaternion.identity);
            //Destroy(hp, 0.2f);
            //
            activeMB.StartCoroutine(PlayTrail(shootSystem.transform.position, hit.point, hit));
        }
        else
        {
            activeMB.StartCoroutine(PlayTrail(shootSystem.transform.position, shootSystem.transform.position + (shootDirection * trailConfig.missDistance), new RaycastHit()));
        }
    }

    private IEnumerator PlayHit(Vector3 pos)
    {
        GameObject ht = hitPool.Get();
        ht.SetActive(true);
        ht.transform.position = pos;
        yield return new WaitForSeconds(0.5f);
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

        if (hit.point != null && hit.collider != null)
        {
            activeMB.StartCoroutine(PlayHit(hit.point));
            if (hit.collider.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                float dmg = damage * StaticData.dmgBuffMult;
                damagable.Damage(dmg);
                DamageText damageText = Instantiate(damageTextPrefab, hit.point, Quaternion.identity);
                damageText.damage = dmg;
            }
        }
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
