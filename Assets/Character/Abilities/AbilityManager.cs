using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Walker2.Controller;

public class AbilityManager : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    [SerializeField]
    private GameObject abilityPrefab;
    
    private IAbility ability;

    private void Start()
    {
        GameObject abl = Instantiate(abilityPrefab, transform);
        SetAbility(abl.GetComponent<IAbility>());
    }

    public void SetAbility(IAbility ability)
    {
        ability.SetPlayer(player);
        this.ability = ability;
        
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q)) 
        {
            
            ability.Fire();
        }
    }
}
