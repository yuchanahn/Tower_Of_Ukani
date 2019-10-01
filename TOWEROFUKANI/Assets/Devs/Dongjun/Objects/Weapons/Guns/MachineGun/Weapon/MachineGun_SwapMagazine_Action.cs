using UnityEngine;

public class MachineGun_SwapMagazine_Action : Gun_SwapMagazine_Action
{
    #region Var: Inspector
    [Header("Ammo Belt")]
    [SerializeField] private Transform ammoBelt;
    [SerializeField] private float ammoBeltAmmoCount;
    #endregion


    public override void OnEnter()
    {
        base.OnEnter();

        ammoBelt.localPosition = new Vector2(0, 0.0625f * ammoBeltAmmoCount);
    }
}
