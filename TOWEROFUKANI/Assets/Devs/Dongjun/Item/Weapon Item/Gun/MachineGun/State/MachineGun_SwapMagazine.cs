using UnityEngine;

public class MachineGun_SwapMagazine : Gun_SwapMagazine_Base<MachineGunItem>
{
    public override void OnEnter()
    {
        base.OnEnter();
        weapon.ammoBelt.localPosition = new Vector2(0, weapon.AmmoBeltMaxY);
    }
}
