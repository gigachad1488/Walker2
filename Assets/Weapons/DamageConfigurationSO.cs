using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "Weapon Config/Damage", order = 1)]
public class DamageConfigurationSO : ScriptableObject
{
    public Vector2Int damageRange;

    public int GetDamage()
    {
        return Random.Range(damageRange.x, damageRange.y + 1);
    }
}
