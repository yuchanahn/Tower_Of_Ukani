using UnityEngine;

public class MachineGun_SwapMagazine_Action : GunSwapMagazine_Base<MachineGun>
{
    public override void OnEnter()
    {
        base.OnEnter();
        gun.ammoBelt.localPosition = new Vector2(0, gun.AmmoBeltMaxY);
    }
}
