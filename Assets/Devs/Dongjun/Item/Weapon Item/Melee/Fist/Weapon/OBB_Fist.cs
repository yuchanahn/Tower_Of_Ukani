using UnityEngine;

[System.Serializable]
public class OBB_Data_Fist : OBB_Data_Animator
{

}

public class OBB_Fist : OBB_Controller_Weapon<OBB_Data_Fist, FistItem>
{
    // States
    private OBB_Melee_Idle state_Idle;
    private OBB_Fist_Dash state_Dash;
    private OBB_Fist_Basic state_Basic;
    private OBB_Fist_Heavy_Charge state_Heavy_Charge;
    private OBB_Fist_Heavy_Punch state_Heavy_Punch;
    private OBB_Fist_Slam state_Slam;

    // Behaviours
    private Single bvr_Idle;
    private Single bvr_Dash;
    private Single bvr_Basic;
    private Sequence bvr_Heavy;
    private Single bvr_Slam;

    protected override void InitStates()
    {
        GetState(ref state_Idle);
        GetState(ref state_Dash);
        GetState(ref state_Basic);
        GetState(ref state_Heavy_Charge);
        GetState(ref state_Heavy_Punch);
        GetState(ref state_Slam);
        SetDefaultState(state_Idle, EMPTY_STATE_ACTION);
    }
    protected override void InitBehaviours()
    {
        bvr_Idle = new Single(
            state_Idle,
            EMPTY_STATE_ACTION,
            () => false);

        bvr_Dash = new Single(
            state_Dash,
            EMPTY_STATE_ACTION,
            () => !GM.Player.IsDashing);

        bvr_Basic = new Single(
            state_Basic,
            EMPTY_STATE_ACTION,
            () => weaponItem.Basic_Dur.IsEnded);

        bvr_Heavy = new Sequence(
            (state_Heavy_Charge,
            EMPTY_STATE_ACTION,
            () => Input.GetKeyUp(PlayerWeaponKeys.SubAbility)),
            (state_Heavy_Punch,
            EMPTY_STATE_ACTION,
            () => weaponItem.Heavy_Dur.IsEnded));

        bvr_Slam = new Single(
            state_Slam,
            EMPTY_STATE_ACTION,
            () => weaponItem.Slam_Dur.IsEnded
               || PlayerInputManager.Inst.Input_DashDir != 0
               || (!weaponItem.Slam_Dur.IsActive && Input.GetKeyDown(PlayerActionKeys.Kick)));
    }
    protected override void InitObjectives()
    {
        // Pause
        NewObjective(
            () => !weaponItem.IsSelected || PlayerStatus.Incapacitated, true)
            .AddBehaviour(bvr_Idle);

        // Dash Attack
        NewObjective(
            () => GM.Player.IsDashing)
            .AddBehaviour(bvr_Dash);

        // Slam Attack
        NewObjective(
            () => !GM.Player.IsKicking
               && !GM.Player.Data.groundDetectionData.isGrounded
               && Input.GetKey(PlayerMovementKeys.FallThrough)
               && PlayerWeaponKeys.GetKeyDown(PlayerWeaponKeys.MainAbility))
            .AddBehaviour(bvr_Slam, true);

        // Basic Attack
        NewObjective(
            () => PlayerWeaponKeys.GetKey(PlayerWeaponKeys.MainAbility))
            .AddBehaviour(bvr_Basic, true);

        // Heavy Attack
        NewObjective(
            () => PlayerWeaponKeys.GetKey(PlayerWeaponKeys.SubAbility))
            .AddBehaviour(bvr_Heavy, true);

        // Default
        SetDefaultObjective()
            .AddBehaviour(bvr_Idle);
    }
}
