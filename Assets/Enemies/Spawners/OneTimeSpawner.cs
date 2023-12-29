using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class OneTimeSpawner : MonoBehaviour
{
    public List<EnemyMovement> enemies = new List<EnemyMovement>();

    public Transform player;

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
        foreach (var enemy in enemies) 
        {
            enemy.player = player;
            enemy.gameObject.SetActive(false);
        }
    }

    public void ActivateEnemies()
    {
        foreach (var enemy in enemies) 
        {
            enemy.gameObject.SetActive(true);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateEnemies();
        }
    }
}
