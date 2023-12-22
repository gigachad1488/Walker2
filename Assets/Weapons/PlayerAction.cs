using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private AbilityManager abilityManager;

    [SerializeField]
    private PlayerGunSelector gunSelector;

    [SerializeField]
    private ProceduralRecoil recoil;

    [SerializeField]
    private Transform reloadArmDest;

    [SerializeField]
    private TextMeshProUGUI currentAmmoText;

    [SerializeField]
    private TextMeshProUGUI maxAmmoText;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private Rig aimRig;
    [SerializeField]
    private Rig armRig;
    [SerializeField]
    private Rig reloadRig;
    [SerializeField]
    private Rig abilityRig;

    private float targetRig = 0;
    private bool reloading = false;
    private float abilityCD;
    private float abilityTargetRig;

    private float defaultFov;
    private float targetFov;

    private float lastShootTime = 0;

    private void Start()
    {
        reloadRig.weight = 0;
        aimRig.weight = 0;
        armRig.weight = 1;
        targetRig = 0;
        abilityCD = 0;

        defaultFov = mainCamera.fieldOfView;
        targetFov = defaultFov;

        maxAmmoText.text = gunSelector.activeGun.maxAmmo.ToString();

        currentAmmoText.text = gunSelector.activeGun.currentAmmo.ToString();
    }

    private void Update()
    {
        if (abilityCD <= 0 && !reloading)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                abilityManager.ShowAbilityIndicator();
                targetFov = defaultFov;
                abilityTargetRig = 1;
                targetRig = 0;
                recoil.aim = false;
            }
            else if (abilityRig.weight >= 0.9f)
            {
                abilityTargetRig = 0;
                abilityCD = abilityManager.cd;
                abilityManager.FireAbility();
            }
        }

        abilityRig.weight = Mathf.Lerp(abilityRig.weight, abilityTargetRig, 10 * Time.deltaTime);
        abilityCD -= Time.deltaTime;

        if (abilityRig.weight < 0.1f)
        {
            if (Input.GetKey(KeyCode.R) && !reloading && gunSelector.activeGun.currentAmmo < gunSelector.activeGun.maxAmmo)
            {
                Reloading();
            }

            if (!reloading)
            {
                if (Mouse.current.rightButton.isPressed)
                {
                    targetRig = 1;
                    targetFov = defaultFov * 0.7f;
                    recoil.aim = true;
                }
                else
                {
                    targetRig = 0;
                    targetFov = defaultFov;
                    recoil.aim = false;
                }
            }
        }

        float aimlerp = Mathf.Lerp(aimRig.weight, targetRig, 15 * Time.deltaTime);
        aimRig.weight = aimlerp;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFov, 15 * Time.deltaTime);

        if (Mouse.current.leftButton.isPressed && gunSelector.activeGun != null && !reloading && gunSelector.activeGun.currentAmmo > 0 && Time.time > gunSelector.activeGun.shootConfig.fireRate + lastShootTime)
        {
            lastShootTime = Time.time;
            gunSelector.activeGun.currentAmmo--;
            currentAmmoText.text = gunSelector.activeGun.currentAmmo.ToString();
            gunSelector.activeGun.Shoot();
            recoil.recoilX = gunSelector.activeGun.shootConfig.spread.x;
            recoil.recoilY = gunSelector.activeGun.shootConfig.spread.y;
            recoil.recoilZ = gunSelector.activeGun.shootConfig.spread.z;
            recoil.kickBackZ = gunSelector.activeGun.shootConfig.kickBack;
            recoil.Recoil();
        }
    }

    private void Reloading()
    {
        reloading = true;
        targetRig = 0;
        targetFov = defaultFov;

        Vector3 magInitPos = gunSelector.weaponIKGrips.magazineTransform.localPosition;

        Vector3[] gunpaths = new Vector3[2];
        gunpaths[0] = new Vector3(gunSelector.activeGunTransform.localPosition.x, gunSelector.activeGunTransform.localPosition.y + 0.1f, gunSelector.activeGunTransform.localPosition.z + 0.05f);
        gunpaths[1] = gunSelector.initPos;

        Vector3[] paths = new Vector3[2];
        paths[0] = reloadArmDest.transform.localPosition;
        paths[1] = gunSelector.weaponIKGrips.magazine.transform.localPosition;
        //paths.Add(gunSelector.weaponIKGrips.leftHandGrip.transform.localPosition);

        Sequence s = DOTween.Sequence(); //hell
        s.Append(gunSelector.activeGunTransform.DOLocalMove(gunpaths[0], 0.3f).SetEase(Ease.OutQuint));
        s.Join(DOVirtual.Float(0, 1, 0.2f, x => reloadRig.weight = x));
        s.Join(gunSelector.reloadArm.DOLocalMove(gunSelector.weaponIKGrips.magazine.transform.localPosition, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Vector3[] p = new Vector3[2];
            p[0] = new Vector3(gunSelector.activeGunTransform.localPosition.x, gunSelector.activeGunTransform.localPosition.y - 0.03f, gunSelector.activeGunTransform.localPosition.z);
            p[1] = new Vector3(gunSelector.activeGunTransform.localPosition.x, gunSelector.activeGunTransform.localPosition.y, gunSelector.activeGunTransform.localPosition.z);

            gunSelector.weaponIKGrips.magazineTransform.SetParent(gunSelector.reloadArm); //grab mag
            gunSelector.activeGun.audioConfig.PlayEmptyClip();
            gunSelector.activeGunTransform.DOLocalPath(p, 0.5f).SetDelay(0.1f).SetEase(Ease.OutCubic);
            gunSelector.reloadArm.DOLocalMove(paths[0], 0.2f).SetEase(Ease.InQuint).OnComplete(() => gunSelector.reloadArm.DOLocalMove(paths[1], 0.2f).SetDelay(0.8f).OnComplete(() =>
            {             
                gunSelector.weaponIKGrips.magazineTransform.SetParent(gunSelector.activeGunTransform); //put mag
                gunSelector.activeGun.audioConfig.PlayReloadClip();
                gunSelector.weaponIKGrips.magazineTransform.localPosition = magInitPos;
                gunSelector.activeGun.currentAmmo = gunSelector.activeGun.maxAmmo;
                currentAmmoText.text = gunSelector.activeGun.currentAmmo.ToString();
                Vector3[] pp = new Vector3[2];
                pp[0] = new Vector3(gunSelector.activeGunTransform.localPosition.x, gunSelector.activeGunTransform.localPosition.y + 0.015f, gunSelector.activeGunTransform.localPosition.z);
                pp[1] = new Vector3(gunSelector.activeGunTransform.localPosition.x, gunSelector.activeGunTransform.localPosition.y, gunSelector.activeGunTransform.localPosition.z);
                gunSelector.activeGunTransform.DOLocalPath(pp, 0.2f).SetEase(Ease.OutCubic);
            }));
        }));
        
        s.Join(gunSelector.activeGunTransform.DOLocalRotateQuaternion(Quaternion.Euler(gunSelector.initRot.eulerAngles.x + 10, gunSelector.initRot.eulerAngles.y, -23), 0.8f).SetEase(Ease.OutQuint)).PrependInterval(0.1f);
        s.AppendInterval(0.8f);
        s.Append(gunSelector.activeGunTransform.DOLocalMove(gunpaths[1], 0.4f));
        s.Join(gunSelector.activeGunTransform.DOLocalRotateQuaternion(gunSelector.initRot, 0.4f).SetEase(Ease.OutQuad));
        s.Join(DOVirtual.Float(1, 0, 0.4f, x => reloadRig.weight = x));
        s.OnComplete(() =>
        {
            reloading = false;
            gunSelector.activeGunTransform.localPosition = gunSelector.initPos;
            gunSelector.activeGunTransform.localRotation = gunSelector.initRot;
        });
        s.Play();      
    }
}
