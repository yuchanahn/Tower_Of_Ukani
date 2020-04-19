using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_IceStaff_Data : OBB_Data_Animator
{
    public bool IsBlizzardActive = false;
}

public class OBB_IceStaff : OBB_Controller_Weapon<OBB_IceStaff_Data, IceStaffItem>
{
    private OBB_IceStaff_Idle state_Idle;
    private OBB_IceStaff_Main state_Main;
    private OBB_IceStaff_Sub state_Sub;
    private OBB_IceStaff_Special state_Special;
    private OBB_IceStaff_Cast state_Cast;

    private Single bvr_Stop;
    private Single bvr_Idle;
    private Single bvr_Main;
    private Sequence bvr_Sub;
    private Sequence bvr_Special;

    protected override void InitStates()
    {
        GetState(ref state_Idle);
        GetState(ref state_Main);
        GetState(ref state_Sub);
        GetState(ref state_Special);
        GetState(ref state_Cast);
        SetDefaultState(state_Idle, EMPTY_STATE_ACTION);
    }
    protected override void InitBehaviours()
    {
        bvr_Stop = new Single(
            state_Idle,
            new StateAction(start: () => Data.IsBlizzardActive = false),
            () => false);

        bvr_Idle = new Single(
            state_Idle,
            EMPTY_STATE_ACTION,
            () => false);

        bvr_Main = new Single(
            state_Main,
            new StateAction(start: () => PlayerStats.Inst.UseMana(weaponItem.Main_ManaUsage.Value)),
            () => state_Main.IsAnimEnded);

        bvr_Sub = new Sequence(
            (state_Cast,
            new StateAction(
                start: () => weaponItem.Sub_CastTime.SetActive(true),
                end: () =>
                {
                    weaponItem.Sub_CastTime.Reset();
                    weaponItem.Sub_CastTime.SetActive(false);
                }),
            () => weaponItem.Sub_CastTime.IsEnded),
            (state_Sub,
            EMPTY_STATE_ACTION,
            () => true));

        bvr_Special = new Sequence(
            (state_Cast,
            new StateAction(
                start: () => weaponItem.Spec_CastTime.SetActive(true),
                end: () => 
                {
                    weaponItem.Spec_CastTime.Reset();
                    weaponItem.Spec_CastTime.SetActive(false);
                }),
            () => weaponItem.Spec_CastTime.IsEnded),
            (state_Special,
            EMPTY_STATE_ACTION,
            () => true));
    }
    protected override void InitObjectives()
    {
        // Stop
        NewObjective(
            () => !weaponItem.IsSelected || PlayerStatus.Incapacitated, true)
            .AddBehaviour(bvr_Stop);

        // Main
        NewObjective(
            () => PlayerWeaponKeys.GetKey(PlayerWeaponKeys.MainAbility)
               && PlayerStats.Inst.HasMana(weaponItem.Main_ManaUsage.Value)
               && weaponItem.Main_CD.IsEnded)
            .AddBehaviour(bvr_Main, true);

        // Sub
        NewObjective(
            () => PlayerWeaponKeys.GetKeyDown(PlayerWeaponKeys.SubAbility)
               && PlayerStats.Inst.HasMana(weaponItem.Sub_ManaUsagePerTick.Value))
            .AddBehaviour(bvr_Sub, true);

        // Special
        NewObjective(
            () => Input.GetKeyDown(PlayerWeaponKeys.SpecialAbility))
            .AddBehaviour(bvr_Special, true);

        // Default
        SetDefaultObjective()
            .AddBehaviour(bvr_Idle);
    }
}
