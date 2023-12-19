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

    public Transform reloadArm;
    public TwoBoneIKConstraint leftHandConst;
    public TwoBoneIKConstraint rightHandConst;
    public TwoBoneIKConstraint reloadLeftHandConst;
    public TwoBoneIKConstraint reloadRightHandConst;
    public RigBuilder builder;

    public WeaponIKGrips weaponIKGrips;

    public GunSO activeGun;
    public Transform activeGunTransform;

    public Vector3 initPos;
    public Quaternion initRot;

    private void Awake()
    {
        GunSO gun = guns.Find(x => x.type == type);

        if (gun == null) 
        {
            return;
        }

        activeGun = gun;
        gun.Spawn(gunParent, this);  
        activeGunTransform = gun.model.transform;
        reloadArm.SetParent(activeGunTransform, false);
        initPos = activeGunTransform.localPosition;
        initRot = activeGunTransform.localRotation;      
        WeaponIKGrips grips = gun.model.GetComponent<WeaponIKGrips>();
        weaponIKGrips = grips;   
        leftHandConst.data.target = grips.leftHandGrip;
        rightHandConst.data.target= grips.rightHandGrip;
        reloadRightHandConst.data.target = grips.rightHandGrip;
        reloadLeftHandConst.data.target = reloadArm;
        builder.Build();    
        //reloadArm.localPosition = leftHandConst.data.target.localPosition;
    }
}
