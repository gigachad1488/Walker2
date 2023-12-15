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

    public bool aim;

    private void Start()
    {
        initialGunPosition = transform.localPosition;
    }

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
        
        Back();
    }

    private void LateUpdate()
    {
        cam.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Recoil()
    {
        if (aim)
        {
            targetPosition -= new Vector3(0, 0, kickBackZ * 0.2f);
            targetRotation += new Vector3(-recoilX * 0.3f, Random.Range(-recoilY * 0.7f, recoilY * 0.7f), Random.Range(-recoilZ * 0.7f, recoilZ * 0.7f));
        }
        else
        {
            targetPosition -= new Vector3(0, 0, kickBackZ);
            targetRotation += new Vector3(-recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        }
    }

    private void Back()
    {
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.fixedDeltaTime);
        transform.localPosition = currentPosition;
    }
    
}
