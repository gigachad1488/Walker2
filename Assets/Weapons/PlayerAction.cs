using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector gunSelector;

    [SerializeField]
    private ProceduralRecoil recoil;

    private void Update()
    {
        if (Mouse.current.leftButton.isPressed && gunSelector.activeGun != null) 
        {
            gunSelector.activeGun.Shoot();
            recoil.recoilX = gunSelector.activeGun.shootConfig.spread.x;
            recoil.recoilY = gunSelector.activeGun.shootConfig.spread.y;
            recoil.recoilZ = gunSelector.activeGun.shootConfig.spread.z;
            recoil.Recoil();
        }
    }
}
