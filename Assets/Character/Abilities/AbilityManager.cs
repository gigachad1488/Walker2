using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public IAbility ability;

    public float cd;

    private void Awake()
    {
        if (abilityPrefab != null)
        {
            GameObject abl = Instantiate(abilityPrefab, transform);
            SetAbility(abl.GetComponent<IAbility>());
        }
        else
        {

        }
    }

    public void SetAbility(IAbility ability)
    {
        if (abilityPrefab != null)
        {
            ability.SetPlayer(player, abilityHand, uiCanvas);
            this.ability = ability;
            cd = ability.Cd;
        }
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
