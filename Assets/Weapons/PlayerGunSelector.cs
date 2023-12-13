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
    private TwoBoneIKConstraint leftHandConst;
    [SerializeField]
    private TwoBoneIKConstraint rightHandConst;
    [SerializeField]
    private RigBuilder builder;

    public GunSO activeGun;

    private void Start()
    {
        GunSO gun = guns.Find(x => x.type == type);

        if (gun == null) 
        {
            return;
        }

        activeGun = gun;
        gun.Spawn(gunParent, this);  
        WeaponIKGrips grips = gun.model.GetComponent<WeaponIKGrips>();
        leftHandConst.data.target = grips.leftHandGrip;
        rightHandConst.data.target= grips.rightHandGrip;
        builder.Build();
    }
}
