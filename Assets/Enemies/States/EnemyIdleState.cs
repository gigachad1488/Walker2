using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    private Vector3 targetPosition;
    private Vector3 direction;
    private float raycastTimer;

    private float afkTime = 2f;

    public EnemyIdleState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        enemy.agent.speed = enemy.speed;
        raycastTimer = 0f;

        ChangeDestination();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.IsAggroed && raycastTimer <= 0f)
        {
            if (Physics.Raycast(enemy.checkPosition.position, (enemy.aggroedPlayer.position - enemy.checkPosition.position).normalized, out RaycastHit hit, 999f, enemy.checkLayer))
            {
                Debug.Log("HITT = " + hit.collider.tag);
                if (hit.collider.CompareTag("Player")) 
                {
                    enemy.stateMachine.ChangeState(enemy.chaseState);
                }             
            }

            raycastTimer = 2f;

            return;
        }

        afkTime -= Time.deltaTime;
        raycastTimer -= Time.deltaTime;

        if (afkTime <= 0)
        {
            if ((enemy.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance && !enemy.agent.pathPending) || enemy.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                ChangeDestination();
                afkTime = 3f;
            }
        }       
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTriggerEvent(AnimationTriggerType animationTriggerType)
    {
        base.AnimationTriggerEvent(animationTriggerType);
    }

    private Vector3 GetRandomPointInCircle()
    {
        return enemy.transform.position + (Vector3)Random.insideUnitCircle * enemy.movementRange;
    }

    private void ChangeDestination()
    {
        targetPosition = GetRandomPointInCircle();
        enemy.agent.destination = targetPosition;
    }
}
