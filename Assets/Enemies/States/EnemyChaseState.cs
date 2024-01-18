using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform player;
    private float moveSpeed = 3;
    private float raycastTimer;

    private float timer = 0.5f;
    public EnemyChaseState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        player = enemy.aggroedPlayer;
        
        enemy.agent.speed = moveSpeed;
        enemy.agent.isStopped = false;

        raycastTimer = 2f;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.IsWithinStrikingDistance && raycastTimer <= 0f)
        {
            if (Physics.Raycast(enemy.checkPosition.position, (enemy.aggroedPlayer.position - enemy.checkPosition.position).normalized, out RaycastHit hit, 999f, enemy.checkLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    enemy.agent.isStopped = true;
                    enemy.stateMachine.ChangeState(enemy.attackState);
                }
               
            }

            raycastTimer = 0.5f;
            return;
            
        }

        timer -= Time.deltaTime;
        raycastTimer -= Time.deltaTime;

        if (timer <= 0)
        {
            enemy.agent.destination = player.position;
            timer = 0.5f;
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
}
