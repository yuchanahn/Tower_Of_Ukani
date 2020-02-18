using UnityEngine;

public class OBB_Gun_Idle_Base<D, W> : AimedWeapon_Idle_Base<D, W>
    where D : OBB_Data_Animator
    where W : GunItem
{
    public override void OnEnter()
    {
        data.Animator.Play(weaponItem.ANIM_Idle);
    }
}
