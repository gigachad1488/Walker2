using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    public Transform player;

    public NavMeshAgent agent;

    public Animator animator;

    public float timeForCheck = 0.5f;

    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        timer = timeForCheck;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            float sqrDistance = (player.position - agent.destination).sqrMagnitude;

            if (sqrDistance > 1) 
            {
                agent.destination = player.position;
            }

            timer = timeForCheck;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
