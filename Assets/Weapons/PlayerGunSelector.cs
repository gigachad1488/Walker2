using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[DisallowMultipleComponent]
public class PlayerGunSelector : MonoBehaviour
{
    [SerializeField]
    private GunType type;

    [SerializeField]
    private Transform gunParent;

    [SerializeField]
    private List<GunSO> guns;

    [SerializeField]
    private List<GunInventory> gunsInInventory;

    public GunSO activeGun;
    public Transform activeGunTransform;
    public WeaponIKGrips weaponIKGrips;
    public Quaternion initRot;
    public Vector3 initPos;

    public Transform reloadArm;
    public TwoBoneIKConstraint leftHandConst;
    public TwoBoneIKConstraint rightHandConst;
    public TwoBoneIKConstraint reloadLeftHandConst;
    public TwoBoneIKConstraint reloadRightHandConst;
    public TwoBoneIKConstraint abilityRightHandConst;
    public TwoBoneIKConstraint switchingRightHandConst;
    public RigBuilder builder;

    private void Awake()
    {
        SpawnWeapons();
        SwitchWeapon(0);
    }

    private void SpawnWeapons()
    {
        gunsInInventory = new List<GunInventory>();

        foreach (GunSO gun in guns)
        {
            GunInventory gunInventory = new GunInventory();
            gunInventory.gun = gun;
            gun.Spawn(gunParent, this);
            gunInventory.gunTransform = gun.model.transform;
            gunInventory.initPos = gunInventory.gunTransform.localPosition;
            gunInventory.initRot = gunInventory.gunTransform.localRotation;
            WeaponIKGrips grips = gun.model.GetComponent<WeaponIKGrips>();
            gunInventory.weaponIKGrips = grips;
            gun.model.SetActive(false);

            gunsInInventory.Add(gunInventory);
        }
    }

    public void SwitchWeapon(int i)
    {
        if (activeGun == null || activeGun.type != gunsInInventory[i].gun.type)
        {
            if (activeGun != null)
            {
                activeGunTransform.localPosition = initPos;
                activeGunTransform.localRotation = initRot;
                activeGun.model.SetActive(false);
            }

            activeGun = gunsInInventory[i].gun;
            activeGun.model.SetActive(true);
            activeGunTransform = activeGun.model.transform;
            weaponIKGrips = gunsInInventory[i].weaponIKGrips;
            initPos = gunsInInventory[i].initPos;
            initRot = gunsInInventory[i].initRot;
            //activeGun.model.transform.localPosition = initPos;
            //activeGun.model.transform.localRotation = initRot;

            reloadArm = weaponIKGrips.reloadAnimArm;
            leftHandConst.data.target = weaponIKGrips.leftHandGrip;
            rightHandConst.data.target = weaponIKGrips.rightHandGrip;
            reloadRightHandConst.data.target = weaponIKGrips.rightHandGrip;
            reloadLeftHandConst.data.target = reloadArm;
            abilityRightHandConst.data.target = weaponIKGrips.rightHandGrip;
            switchingRightHandConst.data.target = weaponIKGrips.rightHandGrip;
            builder.Build();
        }
    }
}

public class GunInventory
{
    public GunSO gun;
    public Transform gunTransform;
    public WeaponIKGrips weaponIKGrips;

    public Vector3 initPos;
    public Quaternion initRot;
}
