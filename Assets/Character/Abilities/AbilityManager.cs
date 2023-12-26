using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Walker2.Controller;

public class AbilityManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private GameObject abilityPrefab;

    [SerializeField]
    private Transform abilityHand;

    [SerializeField]
    private Canvas uiCanvas;

    private bool canFire = true;

    private IAbility ability;

    public float cd;

    private void Awake()
    {
        GameObject abl = Instantiate(abilityPrefab, transform);
        SetAbility(abl.GetComponent<IAbility>());
    }

    public void SetAbility(IAbility ability)
    {
        ability.SetPlayer(player, abilityHand, uiCanvas);
        this.ability = ability;
        cd = ability.Cd;
    }

    public void FireAbility()
    {
        ability.Fire();
    }

    public void ShowAbilityIndicator()
    {
        ability.ShowIndicator();
    }

    public void SetUi(Image ai)
    {
        ability.SetUi(ai);
    }

}
