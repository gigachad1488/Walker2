using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private Transform player;
    private float moveSpeed = 3;

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
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.IsWithinStrikingDistance)
        {
            enemy.agent.isStopped = true;
            enemy.stateMachine.ChangeState(enemy.attackState);
            return;
        }

        timer -= Time.deltaTime;

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
