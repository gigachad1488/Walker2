using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private AbilityManager abilityManager1;
    [SerializeField]
    private AbilityManager abilityManager2;

    [SerializeField]
    private Image ability1UI;
    [SerializeField]
    private Image ability2UI;

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
    private Rig[] handRig;
    [SerializeField]
    private Rig aimRig;
    [SerializeField]
    private Rig armRig;
    [SerializeField]
    private Rig[] reloadRig;
    [SerializeField]
    private Rig abilityRig;
    [SerializeField]
    private Rig[] switchingRig;

    private float targetRig = 0;
    private bool reloading = false;
    private float abilitySwitchCD;
    private float abilityTargetRig;

    private bool ability1 = false;
    private bool ability2 = false;

    private IEnumerator Ability1Recharge;
    private IEnumerator Ability2Recharge;

    private float ability1CD;
    private float ability2CD;

    private int currentRigId = 0;
    private bool switching = false;

    private float defaultFov;
    private float targetFov;

    private float lastShootTime = 0;

    private void Start()
    {
        reloadRig[currentRigId].weight = 0;
        aimRig.weight = 0;
        armRig.weight = 1;
        targetRig = 0;
        ability1CD = 0;
        ability2CD = 0;

        handRig[1].weight = 0;

        defaultFov = mainCamera.fieldOfView;
        targetFov = defaultFov;

        maxAmmoText.text = gunSelector.activeGun.maxAmmo.ToString();

        currentAmmoText.text = gunSelector.activeGun.currentAmmo.ToString();

        abilityManager1.SetUi(ability1UI);
        abilityManager2.SetUi(ability2UI);
    }

    private void Update()
    {
        if (abilitySwitchCD <= 0 && !reloading)
        {
            if (!ability2 && ability1CD <= 0)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    abilityManager1.ShowAbilityIndicator();
                    targetFov = defaultFov;
                    abilityTargetRig = 1;
                    targetRig = 0;
                    recoil.aim = false;
                    ability1 = true;
                }
                else if (abilityRig.weight >= 0.9f)
                {
                    abilityTargetRig = 0;
                    ability1CD = abilityManager1.cd;
                    abilitySwitchCD = 0.3f;
                    abilityManager1.FireAbility();
                    ability1 = false;
                    Ability1Recharge = AbilityRecharge(0);
                    StartCoroutine(Ability1Recharge);
                    goto skipability;
                }
            }

            if (!ability1 && ability2CD <= 0)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    abilityManager2.ShowAbilityIndicator();
                    targetFov = defaultFov;
                    abilityTargetRig = 1;
                    targetRig = 0;
                    recoil.aim = false;                  
                    ability2 = true;
                }
                else if (abilityRig.weight >= 0.9f)
                {
                    abilityTargetRig = 0;
                    ability2CD = abilityManager2.cd;
                    abilitySwitchCD = 0.3f;
                    abilityManager2.FireAbility();
                    ability2 = false;
                    Ability2Recharge = AbilityRecharge(1);
                    StartCoroutine(Ability2Recharge);
                    goto skipability;
                }
            }
        }

        skipability:

        abilityRig.weight = Mathf.Lerp(abilityRig.weight, abilityTargetRig, 10 * Time.deltaTime);
        abilitySwitchCD -= Time.deltaTime;

        if (!switching)
        {
            if (!reloading)
            {
                if (Input.GetKey(KeyCode.Alpha1))
                {
                    SwitchingWeapon(0);
                    return;
                }
                if (Input.GetKey(KeyCode.Alpha2))
                {
                    SwitchingWeapon(1);
                    return;
                }


                if (abilityRig.weight < 0.1f)
                {

                    if (Input.GetKey(KeyCode.R) && !reloading && gunSelector.activeGun.currentAmmo < gunSelector.activeGun.maxAmmo)
                    {
                        Reloading();
                        return;
                    }

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

                if (Mouse.current.leftButton.isPressed && gunSelector.activeGun != null && !reloading && gunSelector.activeGun.currentAmmo > 0 && Time.time > gunSelector.activeGun.shootConfig.fireRate + lastShootTime)
                {
                    lastShootTime = Time.time;
                    gunSelector.activeGun.currentAmmo--;
                    currentAmmoText.text = gunSelector.activeGun.currentAmmo.ToString();
                    gunSelector.activeGun.Shoot();
                    recoil.Recoil();
                }
            }
        }

        float aimlerp = Mathf.Lerp(aimRig.weight, targetRig, 15 * Time.deltaTime);
        aimRig.weight = aimlerp;
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFov, 15 * Time.deltaTime);
    }

    private void SwitchingWeapon(int i)
    {
        if (currentRigId != i)
        {
            switching = true;
            targetRig = 0;
            targetFov = defaultFov;
            recoil.aim = false;
            Quaternion tq = Quaternion.Euler(0, -20, 0);
            Sequence s = DOTween.Sequence();
            s.Append(gunSelector.activeGunTransform.DOLocalMove(new Vector3(gunSelector.activeGunTransform.localPosition.x, gunSelector.activeGunTransform.localPosition.y - 0.3f, gunSelector.activeGunTransform.localPosition.z - 0.3f), 0.5f).OnComplete(() =>
            {
                Vector3 prevt = gunSelector.activeGunTransform.localPosition;
                aimRig.weight = 0;
                armRig.weight = 0;
                reloadRig[currentRigId].weight = 0;
                handRig[currentRigId].weight = 0;
                switchingRig[currentRigId].weight = 0;
                gunSelector.SwitchWeapon(i);
                handRig[i].weight = 1;
                gunSelector.activeGunTransform.localPosition = prevt;
                gunSelector.activeGunTransform.localRotation = gunSelector.initRot * tq;
                currentAmmoText.text = gunSelector.activeGun.currentAmmo.ToString();
                maxAmmoText.text = gunSelector.activeGun.maxAmmo.ToString();
                currentRigId = i;
            }));
            s.Join(gunSelector.activeGunTransform.DOLocalRotateQuaternion(gunSelector.activeGunTransform.localRotation * tq, 0.5f).SetEase(Ease.OutQuint));
            s.Join(DOVirtual.Float(0, 1, 0.4f, x => switchingRig[currentRigId].weight = x).SetDelay(0.2f));
            s.Join(DOVirtual.Float(1, 0, 0.6f, x => armRig.weight = x));
            s.Play();
            s.OnComplete(() =>
            {
                Sequence ss = DOTween.Sequence();
                ss.Append(gunSelector.activeGunTransform.DOLocalMove(gunSelector.initPos, 0.4f));
                ss.Join(gunSelector.activeGunTransform.DOLocalRotateQuaternion(gunSelector.initRot, 0.4f).SetEase(Ease.OutQuad));
                ss.Join(DOVirtual.Float(1, 0, 0.4f, x => switchingRig[i].weight = x));
                ss.Join(DOVirtual.Float(0, 1, 0.4f, x => armRig.weight = x));
                ss.OnComplete(() =>
                {
                    gunSelector.activeGunTransform.localPosition = gunSelector.initPos;
                    gunSelector.activeGunTransform.localRotation = gunSelector.initRot;
                    armRig.weight = 1;
                    switching = false;

                    recoil.recoilX = gunSelector.activeGun.shootConfig.spread.x;
                    recoil.recoilY = gunSelector.activeGun.shootConfig.spread.y;
                    recoil.recoilZ = gunSelector.activeGun.shootConfig.spread.z;
                    recoil.kickBackZ = gunSelector.activeGun.shootConfig.kickBack;

                });
                ss.Play();
            });
        }
    }

    private void Reloading()
    {
        reloading = true;
        targetRig = 0;
        targetFov = defaultFov;

        Debug.Log("NAMEE = " + gunSelector.activeGunTransform.name);

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
        s.Join(DOVirtual.Float(0, 1, 0.2f, x => reloadRig[currentRigId].weight = x));
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
        s.Join(DOVirtual.Float(1, 0, 0.4f, x => reloadRig[currentRigId].weight = x));
        s.OnComplete(() =>
        {
            reloading = false;
            gunSelector.activeGunTransform.localPosition = gunSelector.initPos;
            gunSelector.activeGunTransform.localRotation = gunSelector.initRot;
        });
        s.Play();
    }

    private IEnumerator AbilityRecharge(int abilitySlot)
    {
        WaitForSeconds wfs = new WaitForSeconds(0.2f);
        float cd = 0;
        float timer = 0;

        Image abilityImage = ability1UI;

        if (abilitySlot == 0) 
        {
            abilityImage = ability1UI;
            cd = ability1CD;
        }
        else if (abilitySlot == 1) 
        {
            abilityImage = ability2UI;
            cd = ability2CD;
        }

        abilityImage.color = Color.gray;

        while (timer <= cd)
        {
            timer += 0.2f;
            abilityImage.fillAmount = timer / cd;
            yield return wfs;
        }

        if (abilitySlot == 0)
        {
            ability1CD = 0;
        }
        else if (abilitySlot == 1)
        {
            ability2CD = 0;
        }

        abilityImage.color = Color.white;
    }
}
