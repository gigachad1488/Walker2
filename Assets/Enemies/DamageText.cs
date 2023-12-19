using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float damage;

    public bool crit = false;

    public TextMeshPro damageText;

    private void Start()
    {
        damageText.text = math.round(damage).ToString();

        float xRand = UnityEngine.Random.Range(-0.5f, 0.5f);
        float zRand = UnityEngine.Random.Range(-0.5f, 0.5f);

        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        Quaternion rot = Quaternion.Euler(xRand * 30, 0, zRand * 30);
        transform.rotation *= rot;
        transform.position = new Vector3(transform.position.x + xRand, transform.position.y, transform.position.z + zRand);

        if (crit) 
        {
            damageText.color = Color.red;
            damageText.fontSize *= 1.3f;
        }
        else
        {
            damageText.color = Color.yellow;
        }

        transform.DOBlendableLocalMoveBy(transform.up * 1.1f, 0.5f).SetEase(Ease.OutExpo).OnComplete(() => Destroy(gameObject));
    }
}
