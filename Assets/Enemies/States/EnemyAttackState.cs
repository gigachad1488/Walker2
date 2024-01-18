using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float fireTimer;

    private float exitTime;

    private float reloadTimer;
    private bool reloading;

    private float raycastTimer;
    public EnemyAttackState(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        exitTime = 3f;
        fireTimer = enemy.gun.shootConfig.fireRate;
        reloadTimer = 3f;
        enemy.gun.currentAmmo = enemy.gun.maxAmmo;
        reloading = false;

        raycastTimer = 2f;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Quaternion lookRot = Quaternion.LookRotation(enemy.aggroedPlayer.position - enemy.checkPosition.position);
        enemy.transform.eulerAngles = new Vector3(0, lookRot.eulerAngles.y, 0);

        if (raycastTimer <= 0f)
        {
            if (Physics.Raycast(enemy.checkPosition.position, (enemy.aggroedPlayer.position - enemy.checkPosition.position).normalized, out RaycastHit hit, 999f, enemy.checkLayer))
            {
                if (!hit.collider.CompareTag("Player"))
                {
                    enemy.stateMachine.ChangeState(enemy.chaseState);
                }
            }

            raycastTimer = 2f;
        }

        if (enemy.IsWithinStrikingDistance)
        {
            exitTime = 3f;
        }

        if (exitTime <= 0f)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
            return;
        }

        raycastTimer -= Time.deltaTime;
        fireTimer -= Time.deltaTime;
        reloadTimer -= Time.deltaTime;
        exitTime -= Time.deltaTime;

        if (enemy.gun.currentAmmo > 0)
        {
            if (fireTimer <= 0f)
            {
                enemy.gunParent.eulerAngles = new Vector3(lookRot.eulerAngles.x + Random.Range(-2f, 2f), lookRot.eulerAngles.y + Random.Range(-2f, 2f), lookRot.eulerAngles.z);
                enemy.gun.Shoot();
                fireTimer = enemy.gun.shootConfig.fireRate;
                enemy.gun.currentAmmo--;
            }
        }
        else if (reloadTimer <= 0f && !reloading)
        {
            reloadTimer = 3f;
            reloading = true;
        }
        else if (reloadTimer <= 0f && reloading)
        {
            enemy.gun.currentAmmo = enemy.gun.maxAmmo;
            reloading = false;
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

    private void Reload()
    {
        enemy.gun.currentAmmo = enemy.gun.maxAmmo;
    }
}
