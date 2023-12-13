using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Weapon Config/Trail", order = 1)]
public class TrailConfigurationSO : ScriptableObject
{
    public Material material;
    public AnimationCurve widthCurve;
    public float duration = 0.5f;
    public float minVertexDistance = 0.1f;
    public Gradient color;
    public float missDistance = 100;
    public float simulationSpeed = 100;

}
