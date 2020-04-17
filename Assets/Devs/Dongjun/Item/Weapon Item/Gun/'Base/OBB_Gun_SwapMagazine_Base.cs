using Dongjun.Helper;
using UnityEngine;

public class OBB_Gun_SwapMagazine_Base<D, W> : AimedWeapon_State_Base<D, W>
    where D : OBB_Data_Animator
    where W : GunItem
{
    [Header("Effect")]
    [SerializeField] protected Transform magazineDropPos;
    [SerializeField] protected PoolingObj droppedMagazinePrefab;

    public override void OnEnter()
    {
        // Timer
        weaponItem.Main_SwapMagazine_Dur.SetActive(true);

        // Animation
        data.Animator.Play(weaponItem.ANIM_SwapMagazine);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Main_SwapMagazine_Dur.EndTime.Value);
    }
    public override void OnExit()
    {
        // Timer
        weaponItem.Main_SwapMagazine_Dur.SetActive(false);
        weaponItem.Main_SwapMagazine_Dur.Reset();

        // Animation
        data.Animator.ResetSpeed();
    }

    protected virtual void AnimEvent_DropMagazine()
    {
        if (droppedMagazinePrefab != null)
            ObjPoolingManager.Spawn(droppedMagazinePrefab, magazineDropPos.position, transform.rotation);
    }
}
