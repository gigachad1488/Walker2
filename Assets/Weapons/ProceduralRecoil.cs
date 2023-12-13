using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    [SerializeField]
    private Transform cam;

    public float recoilX;
    public float recoilY;
    public float recoilZ;
    public float kickBackZ;
    public float snappiness;
    public float returnSpeed;

    private Vector3 currentRotation;
    private Vector3 currentPosition;
    private Vector3 targetRotation;
    private Vector3 targetPosition;
    private Vector3 initialGunPosition;

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Recoil()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
