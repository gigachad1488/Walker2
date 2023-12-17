using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector gunSelector;

    [SerializeField]
    private ProceduralRecoil recoil;

    [SerializeField]
    private Transform reloadArmDest;

    [SerializeField]
    private Rig aimRig;
    [SerializeField]
    private Rig armRig;

    [SerializeField]
    private Transform reloadArm;

    private float targetRig = 0;
    private bool reloading = false;

    private void Update()
    {
        if (Input.GetKey(KeyCode.R) && !reloading)
        {
            Reloading();
        }

        if (!reloading)
        {
            if (Mouse.current.rightButton.isPressed)
            {
                targetRig = 1;
                recoil.aim = true;
            }
            else
            {
                targetRig = 0;
                recoil.aim = false;
            }
        }

        float aimlerp = Mathf.Lerp(aimRig.weight, targetRig, 15 * Time.deltaTime);
        aimRig.weight = aimlerp;

        if (Mouse.current.leftButton.isPressed && gunSelector.activeGun != null && !reloading)
        {
            gunSelector.activeGun.Shoot();
            recoil.recoilX = gunSelector.activeGun.shootConfig.spread.x;
            recoil.recoilY = gunSelector.activeGun.shootConfig.spread.y;
            recoil.recoilZ = gunSelector.activeGun.shootConfig.spread.z;
            recoil.Recoil();
        }
    }

    private void Reloading()
    {
        reloading = true;
        targetRig = 0;

        List<Vector3> gunpaths = new List<Vector3>();
        gunpaths.Add(new Vector3(gunSelector.activeGunTransform.localPosition.x, gunSelector.activeGunTransform.localPosition.y + 0.1f, gunSelector.activeGunTransform.localPosition.z + 0.1f));
        gunpaths.Add(gunSelector.initPos);

        gunSelector.activeGunTransform.DOLocalMove(gunpaths[0], 1.3f).SetEase(Ease.OutQuint).OnComplete(() =>
        {
            gunSelector.activeGunTransform.DOLocalMove(gunpaths[1], 0.6f).OnComplete(() =>
            {
                reloading = false;
                gunSelector.leftHandConst.data.target = gunSelector.weaponIKGrips.leftHandGrip;
                gunSelector.builder.Build();
                gunSelector.activeGunTransform.localPosition = gunSelector.initPos;
                gunSelector.activeGunTransform.localRotation = gunSelector.initRot;
                targetRig = 1;
            });
            
        });
        //gunSelector.activeGunTransform.localPosition = new Vector3(gunSelector.activeGunTransform.localPosition.x, gunSelector.activeGunTransform.localPosition.y + 0.1f, gunSelector.activeGunTransform.localPosition.z + 0.1f);
        gunSelector.activeGunTransform.localRotation = Quaternion.Euler(gunSelector.initRot.eulerAngles.x, gunSelector.initRot.eulerAngles.y, -23);
        reloadArm.SetParent(gunSelector.activeGunTransform, false);
        reloadArm.localPosition = gunSelector.leftHandConst.data.target.localPosition;
        gunSelector.leftHandConst.data.target = reloadArm;
        gunSelector.builder.Build();
        List<Vector3> paths = new List<Vector3>();
        paths.Add(gunSelector.weaponIKGrips.magazine.transform.localPosition);
        paths.Add(reloadArmDest.transform.localPosition);
        paths.Add(gunSelector.weaponIKGrips.magazine.transform.localPosition);
        reloadArm.DOLocalPath(paths.ToArray(), 1.3f).SetEase(Ease.InOutQuad).OnComplete(() =>
        {

        });
    }
}
