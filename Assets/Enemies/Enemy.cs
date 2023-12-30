using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyHealth))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, ITriggerCheckable
{
    [HideInInspector]
    public EnemyHealth health;
    [HideInInspector]
    public NavMeshAgent agent;
    [SerializeField]
    private GunSO gunSO;
    [HideInInspector]
    public GunSO gun;

    public LayerMask hitMask;

    public Transform gunParent;

    public EnemyStateMachine stateMachine;
    public EnemyIdleState idleState;
    public EnemyChaseState chaseState;
    public EnemyAttackState attackState;
    public EnemyState noState;

    public float movementRange = 5;
    public float speed = 2f;

    public Transform aggroedPlayer;

    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    private void Awake()
    {
        gun = Instantiate(gunSO);

        health = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();

        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        noState = new EnemyState(this, stateMachine);

        gun.Spawn(gunParent, this);
        gun.shootConfig.hitMask = hitMask;

        health.onDeath += OnDeath;
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

    private void OnDeath(Vector3 direction)
    {
        agent.isStopped = true;
        stateMachine.ChangeState(noState);
    }

}
