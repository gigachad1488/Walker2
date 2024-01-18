using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HealthBar))]
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

    public HealthBar healthBar;
    public Canvas canvas;

    public float movementRange = 5;
    public float speed = 2f;

    public Transform aggroedPlayer;
    public LayerMask checkLayer;
    public Transform checkPosition;

    public bool IsAggroed { get; set; }
    public bool IsWithinStrikingDistance { get; set; }

    private void Awake()
    {
        healthBar = GetComponent<HealthBar>();

        gun = Instantiate(gunSO);

        health = GetComponent<EnemyHealth>();
        agent = GetComponent<NavMeshAgent>();

        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
        noState = new EnemyState(this, stateMachine);
        canvas.gameObject.SetActive(false);

        gun.Spawn(gunParent, this);
        gun.shootConfig.hitMask = hitMask;

        health.onDeath += OnDeath;
        health.onTakeDamage += OnDamage;

        healthBar.Set(1, 1);
    }

    private void OnDamage(int damage, Vector3 position)
    {
        healthBar.Change(health.CurrentHealth / (float)health.MaxHealth);

        if (!aggroedPlayer)
        {
            aggroedPlayer = LevelManager.instance.player.transform;
            stateMachine.ChangeState(chaseState);
        }
    }

    private void Start()
    {
        healthBar.Visible(false);
        stateMachine.Initialize(idleState);
    }

    private void Update()
    {
        stateMachine.CurrentEnemyState.FrameUpdate();

        if (aggroedPlayer != null) 
        {
            healthBar.Visible(true);
            canvas.gameObject.SetActive(true);
            canvas.transform.LookAt(aggroedPlayer.transform);
        }
    }

    public void AnimationTriggerEvent(AnimationTriggerType type)
    {
        stateMachine.CurrentEnemyState.AnimationTriggerEvent(type);
    }

    private void OnDeath(Vector3 direction)
    {
        agent.isStopped = true;
        stateMachine.ChangeState(noState);
        canvas.gameObject.SetActive(false);
        this.enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (aggroedPlayer != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(checkPosition.position, (aggroedPlayer.position - checkPosition.position).normalized * 20);
        }
    }

}
