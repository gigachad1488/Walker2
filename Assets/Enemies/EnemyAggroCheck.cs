using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Walker2.Controller;

[RequireComponent(typeof(Collider))]
public class EnemyAggroCheck : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.aggroedPlayer = other.GetComponentInParent<PlayerController>().center;
            enemy.IsAggroed = true;          
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.IsAggroed = false;
        }
    }
}
