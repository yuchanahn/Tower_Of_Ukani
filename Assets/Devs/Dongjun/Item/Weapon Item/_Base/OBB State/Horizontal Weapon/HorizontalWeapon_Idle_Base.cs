using UnityEngine;

public class HorizontalWeapon_Idle_Base<D, W> : Weapon_State_Base<D, W>
    where D : OBB_Data_Animator
    where W : WeaponItem
{
    public override void OnLateUpdate()
    {
        if (PlayerStatus.Incapacitated)
        {
            // Look At Knockback Dir
            if (PlayerStatus.IsKnockbacked)
                Flip_Logic.FlipXTo(-(int)Mathf.Sign(PlayerStatus.KnockbackDir.x), transform);

            return;
        }

        // Look At Mouse
        transform.LookAtMouseFlipX(Global.Inst.MainCam, transform);
    }
}
