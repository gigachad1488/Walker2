using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage", menuName = "Weapon Config/Damage", order = 1)]
public class DamageConfigurationSO : ScriptableObject
{
    public Vector2 damageRange;
    public float force;
    public int GetDamage()
    {
        Vector2 dmgWithMult = damageRange * StaticData.dmgBuffMult;
        return Convert.ToInt32(UnityEngine.Random.Range(dmgWithMult.x, dmgWithMult.y + 1));
    }
}
