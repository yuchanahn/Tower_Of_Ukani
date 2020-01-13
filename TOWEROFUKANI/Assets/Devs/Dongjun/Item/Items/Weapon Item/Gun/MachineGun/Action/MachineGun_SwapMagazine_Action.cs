using UnityEngine;

public class MachineGun_SwapMagazine_Action : GunSwapMagazine_Base<MachineGunItem>
{
    public override void OnEnter()
    {
        base.OnEnter();
        weapon.ammoBelt.localPosition = new Vector2(0, weapon.AmmoBeltMaxY);
    }
}
