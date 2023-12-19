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
    private float abilityHoldTime = 0;
    private float abilityHoldAddTime = 0;

    private float lastShootTime = 0;

    private int maxAmmo;
    private int currentAmmo;

    private void Start()
    {
        reloadRig.weight = 0;
        aimRig.weight = 0;
        armRig.weight = 1;
        targetRig = 0;
        abilityCD = 0;

        maxAmmo = gunSelector.activeGun.ammo;
        maxAmmoText.text = maxAmmo.ToString();
        currentAmmo = maxAmmo;
        currentAmmoText.text = currentAmmo.ToString();
    }

    private void Update()
    {
        if (abilityCD <= 0 && !reloading)
        {
            abilityHoldTime += abilityHoldAddTime;
            if (Input.GetKey(KeyCode.Q))
            {
                abilityManager.ParticlesVisible(true);
                abilityHoldAddTime = Time.deltaTime;
                abilityTargetRig = 1;
                targetRig = 0;
                recoil.aim = false;
            }
            else if (abilityHoldTime > 0.4f)
            {
                abilityManager.ParticlesVisible(false);
                abilityTargetRig = 0;
                abilityHoldTime = 0;
                abilityHoldAddTime = 0;
                abilityCD = abilityManager.cd;
                abilityManager.FireAbility();
            }
        }

        abilityRig.weight = Mathf.Lerp(abilityRig.weight, abilityTargetRig, 10 * Time.deltaTime);
        abilityCD -= Time.deltaTime;

        if (abilityRig.weight < 0.1f)
        {
            if (Input.GetKey(KeyCode.R) && !reloading && currentAmmo < maxAmmo)
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
        }

        float aimlerp = Mathf.Lerp(aimRig.weight, targetRig, 15 * Time.deltaTime);
        aimRig.weight = aimlerp;

        if (Mouse.current.leftButton.isPressed && gunSelector.activeGun != null && !reloading && currentAmmo > 0 && Time.time > gunSelector.activeGun.shootConfig.fireRate + lastShootTime)
        {
            lastShootTime = Time.time;
            currentAmmo--;
            currentAmmoText.text = currentAmmo.ToString();
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

        Vector3 magInitPos = gunSelector.weaponIKGrips.magazineTransform.localPosition;

        List<Vector3> gunpaths = new List<Vector3>();
        gunpaths.Add(new Vector3(gunSelector.activeGunTransform.localPosition.x, gunSelector.activeGunTransform.localPosition.y + 0.1f, gunSelector.activeGunTransform.localPosition.z + 0.05f));
        gunpaths.Add(gunSelector.initPos);

        List<Vector3> paths = new List<Vector3>();
        paths.Add(reloadArmDest.transform.localPosition);
        paths.Add(gunSelector.weaponIKGrips.magazine.transform.localPosition);
        //paths.Add(gunSelector.weaponIKGrips.leftHandGrip.transform.localPosition);

        Sequence s = DOTween.Sequence(); //hell
        s.Append(gunSelector.activeGunTransform.DOLocalMove(gunpaths[0], 1f).SetEase(Ease.OutQuint));
        s.Join(DOVirtual.Float(0, 1, 0.2f, x => reloadRig.weight = x));
        s.Join(gunSelector.reloadArm.DOLocalMove(gunSelector.weaponIKGrips.magazine.transform.localPosition, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            gunSelector.weaponIKGrips.magazineTransform.SetParent(gunSelector.reloadArm);
            gunSelector.reloadArm.DOLocalMove(paths[0], 0.3f).SetEase(Ease.InQuint).OnComplete(() => gunSelector.reloadArm.DOLocalMove(paths[1], 0.4f).SetDelay(0.3f).OnComplete(() =>
            {
                gunSelector.weaponIKGrips.magazineTransform.SetParent(gunSelector.activeGunTransform);
                gunSelector.weaponIKGrips.magazineTransform.localPosition = magInitPos;
                currentAmmo = maxAmmo;
                currentAmmoText.text = currentAmmo.ToString();
            }));
        }));
        
        s.Join(gunSelector.activeGunTransform.DOLocalRotateQuaternion(Quaternion.Euler(gunSelector.initRot.eulerAngles.x + 10, gunSelector.initRot.eulerAngles.y, -23), 0.8f).SetEase(Ease.OutQuint)).PrependInterval(0.1f);
        s.AppendInterval(0.2f);
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
