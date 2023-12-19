using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot Config", menuName = "Weapon Config/Shoot", order = 2)]
public class ShootConfigurationSO : ScriptableObject
{
    public LayerMask hitMask;
    public Vector3 spread = new Vector3(0.1f, 0.1f, 0.1f);
    public float fireRate = 0.5f;
    public float kickBack = 0.5f;

    public Vector3 GetSpread(float shootTime = 0)
    {
        Vector3 shootDirection = new Vector3(Random.Range(spread.x, spread.x), Random.Range(spread.y, spread.y), Random.Range(spread.z, spread.z));
        shootDirection.Normalize();

        return shootDirection;
    }
}
