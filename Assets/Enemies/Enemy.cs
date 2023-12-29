using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, ITriggerCheckable
{
    public EnemyHealth health;
    public NavMeshAgent agent;

    public EnemyStateMachine stateMachine;
    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;   

    public float movementRange = 5;
    public float speed = 2f;

    public Transform aggroedPlayer;

    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();

        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
    }

    private void Start()
    {
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();
    }

    public void AnimationTriggerEvent(AnimationTriggerType type)
    {
        stateMachine.CurrentEnemyState.AnimationTriggerEvent(type);
    }

}
