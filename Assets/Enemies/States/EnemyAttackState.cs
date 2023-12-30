using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    private float fireTimer;

    private float exitTime;

    private float reloadTimer;
    private bool reloading;
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
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        Quaternion lookRot = Quaternion.LookRotation(enemy.aggroedPlayer.position - enemy.transform.position);
        enemy.transform.eulerAngles = new Vector3(0, lookRot.eulerAngles.y, 0);
        

        if (enemy.IsWithinStrikingDistance)
        {
            exitTime = 3f;
        }

        if (exitTime <= 0)
        {
            enemy.stateMachine.ChangeState(enemy.chaseState);
            return;
        }

        fireTimer -= Time.deltaTime;
        reloadTimer -= Time.deltaTime;
        exitTime -= Time.deltaTime;

        if (enemy.gun.currentAmmo > 0)
        {
            if (fireTimer <= 0)
            {
                enemy.gunParent.eulerAngles = new Vector3(lookRot.eulerAngles.x + Random.Range(-2f, 2f), lookRot.eulerAngles.y + Random.Range(-2f, 2f), lookRot.eulerAngles.z);
                enemy.gun.Shoot();
                fireTimer = enemy.gun.shootConfig.fireRate;
                enemy.gun.currentAmmo--;
            }
        }
        else if (reloadTimer <= 0 && !reloading)
        {
            reloadTimer = 3f;
            reloading = true;
        }
        else if (reloadTimer <= 0 && reloading)
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
