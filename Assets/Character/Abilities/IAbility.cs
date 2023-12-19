using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Walker2.Controller;

public interface IAbility
{
    public void SetPlayer(PlayerController player)
    { }

    public void Fire()
    { }

    public float Value { get; }
    public float Cd { get; }

    public float Duration { get; }
    
    public GameObject AbilityParticles { get; }
}
