using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_IceStaff_Data : OBB_Data_Animator
{

}

public class OBB_IceStaff : OBB_Controller_Weapon<OBB_IceStaff_Data, IceStaffItem>
{
    private OBB_IceStaff_Idle state_Idle;
    private OBB_IceStaff_Main state_Main;
    private OBB_IceStaff_Sub state_Sub;
    private OBB_IceStaff_Special state_Special;
    private OBB_IceStaff_Cast state_Cast;

    private Single bvr_Idle;
    private Single bvr_Main;
    private Sequence bvr_Sub;
    private Sequence bvr_Special;

    protected override void InitStates()
    {
        GetState(ref state_Idle);
        GetState(ref state_Main);
        //GetState(ref state_Sub);
        //GetState(ref state_Special);
        //GetState(ref state_Cast);
        SetDefaultState(state_Idle, EMPTY_STATE_ACTION);
    }
    protected override void InitBehaviours()
    {
        bvr_Idle = new Single(
            state_Idle,
            EMPTY_STATE_ACTION,
            () => false);

        bvr_Main = new Single(
            state_Main,
            new StateAction(
                start: () => PlayerStats.Inst.UseMana(weaponItem.ManaUsage_Main_Shoot.Value)),
            () => state_Main.IsAnimEnded);

        //bvr_Sub = new Single();

        //bvr_Special = new Single();
    }
    protected override void InitObjectives()
    {
        // Pause
        NewObjective(
            () => !weaponItem.IsSelected || PlayerStatus.Incapacitated, true)
            .AddBehaviour(bvr_Idle);

        // Main
        NewObjective(
            () => PlayerWeaponKeys.GetKey(PlayerWeaponKeys.MainAbility)
               && PlayerStats.Inst.HasMana(weaponItem.ManaUsage_Main_Shoot.Value)
               && weaponItem.CD_Main_Shoot.IsEnded)
            .AddBehaviour(bvr_Main, true);

        SetDefaultObjective()
            .AddBehaviour(bvr_Idle);
    }
}
