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

    public List<GunInventory> gunsInInventory;

    public GunSO activeGun;
    public Transform activeGunTransform;
    public WeaponIKGrips weaponIKGrips;
    public Quaternion initRot;
    public Vector3 initPos;

    public int currentGunId;

    public Transform reloadArm;
    public TwoBoneIKConstraint[] leftHandConst = new TwoBoneIKConstraint[2];
    public TwoBoneIKConstraint[] rightHandConst = new TwoBoneIKConstraint[2];
    public TwoBoneIKConstraint[] reloadLeftHandConst = new TwoBoneIKConstraint[2];
    public TwoBoneIKConstraint[] reloadRightHandConst = new TwoBoneIKConstraint[2];
    public TwoBoneIKConstraint[] abilityRightHandConst = new TwoBoneIKConstraint[2];
    public TwoBoneIKConstraint[] switchingRightHandConst = new TwoBoneIKConstraint[2];
    public RigBuilder builder;

    private void Awake()
    {
        currentGunId = 0;
        SpawnWeapons();
        activeGun = gunsInInventory[0].gun;
    }

    private void Start()
    {
        gunsInInventory[0].gun.model.SetActive(false);
        gunsInInventory[1].gun.model.SetActive(false);

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
            leftHandConst[currentGunId].data.target = grips.leftHandGrip;
            rightHandConst[currentGunId].data.target = grips.rightHandGrip;
            reloadRightHandConst[currentGunId].data.target = grips.rightHandGrip;
            reloadLeftHandConst[currentGunId].data.target = grips.reloadAnimArm;
            //abilityRightHandConst[currentGunId].data.target = grips.rightHandGrip;
            switchingRightHandConst[currentGunId].data.target = grips.rightHandGrip;          
            //gun.model.SetActive(false);

            gunsInInventory.Add(gunInventory);

            currentGunId++;
        }

        builder.Build();
      
    }

    public void SwitchWeapon(int i)
    {
        //builder.Build();
        if (activeGunTransform == null || activeGun.type != gunsInInventory[i].gun.type)
        {
            if (activeGunTransform != null)
            {
                activeGunTransform.localPosition = initPos;
                activeGunTransform.localRotation = initRot;
                activeGun.model.SetActive(false);
            }

            activeGun = gunsInInventory[i].gun;
            currentGunId = i;
            activeGun.model.SetActive(true);
            activeGunTransform = activeGun.model.transform;
            weaponIKGrips = gunsInInventory[i].weaponIKGrips;
            initPos = gunsInInventory[i].initPos;
            initRot = gunsInInventory[i].initRot;
            //activeGun.model.transform.localPosition = initPos;
            //activeGun.model.transform.localRotation = initRot;

            reloadArm = weaponIKGrips.reloadAnimArm;           
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
