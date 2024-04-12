using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IAbility
{
    public void SetPlayer(PlayerController player, Transform abilityHand, Canvas uiCanvas)
    { }

    public void Fire()
    { }

    public void ShowIndicator()
    { }

    public float Value { get; }
    public float Cd { get; }

    public float Duration { get; }
    
    public GameObject AbilityParticlesPrefab { get; }

    public Sprite AbilitySprite { get; }

    public void SetUi(Image ai)
    { }
}
