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
    private Rig aimRig;
    [SerializeField]
    private Rig armRig;

    private float targetRig = 0;

    private void Update()
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

        float aimlerp = Mathf.Lerp(aimRig.weight, targetRig, 15 * Time.deltaTime);
        aimRig.weight = aimlerp;

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
