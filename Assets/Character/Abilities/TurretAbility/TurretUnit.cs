using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class TurretUnit : MonoBehaviour
{
    [SerializeField]
    private Transform shootPoint;

    [SerializeField]
    private float attackRange = 5;   

    private float attackRate = 1f;
    private float duration = 10f;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private LayerMask layers;

    [SerializeField]
    private TurretBeam beamPrefab;

    [SerializeField]
    private float force = 2000;

    private int damage = 20;

    public void Deploy(float duration, int damage)
    {
        this.duration = duration;
        this.damage = damage;

        Invoke(nameof(Destroy), duration);
        StartCoroutine(Shooting());
    }

    public void Fire(RaycastHit hit, Vector3 direction)
    {
        TurretBeam t = Instantiate(beamPrefab, shootPoint.position, Quaternion.LookRotation(direction));
        float distance = Vector3.Distance(t.transform.position, hit.point);

        t.transform.localScale = new Vector3(1, 1, 1 * (distance * 6.3f * 0.01f));

        hit.transform.GetComponent<HitBox>().OnHit(damage, force, hit.point, direction);
    }

    private IEnumerator Shooting()
    {
        WaitForSeconds wfs = new WaitForSeconds(attackRate);
        while (true) 
        {
            Collider[] colliders = Physics.OverlapSphere(shootPoint.position, attackRange, enemyLayer);

            FireClosestEnemyInView(colliders);

            yield return wfs;
        }
    }

    private void FireClosestEnemyInView(Collider[] colliders)
    {
        if (colliders.Length <= 0)
        {
            return;
        }

        float distance = 0f;
        float minDistance = 999f;

        int enemyId = -1;

        for (int i = 0; i < colliders.Length; i++)
        {
            float dst = Vector3.Distance(colliders[i].transform.position, shootPoint.position);

            if (dst < minDistance)
            {
                distance = dst;
                enemyId = i;
            }
        }

        Vector3 direction = (colliders[enemyId].transform.position - shootPoint.position).normalized;

        if (Physics.Raycast(shootPoint.position, direction, out RaycastHit hit, distance, layers))
        {
            if ((enemyLayer & 1 << hit.transform.gameObject.layer) == 1 << hit.transform.gameObject.layer)
            {
                Fire(hit, direction);
            }
            else
            {
                FireClosestEnemyInView(Array.FindAll(colliders, x => !x.Equals(colliders[enemyId])));
            }        
        }
        else
        {
            FireClosestEnemyInView(Array.FindAll(colliders, x => !x.Equals(colliders[enemyId])));
        }       
    }

    private void Destroy()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(200, 0, 0, 0.1f);
        Gizmos.DrawSphere(shootPoint.position, attackRange);
    }
}
