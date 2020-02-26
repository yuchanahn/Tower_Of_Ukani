using UnityEngine;

public class OBB_Data_WoodenShortbow : OBB_Data_Animator
{

}

public class OBB_WoodenShortbow : OBB_Controller_Weapon<OBB_Data_WoodenShortbow, WoodenShortbowItem>
{
    // States
    private OBB_Bow_Idle state_Idle;
    private OBB_Bow_Draw state_Draw;
    private OBB_WoodenShortbow_Shoot state_Shoot;

    // Behaviours
    private Single bvr_Idle;
    private Sequence bvr_Shoot;

    protected override void InitStates()
    {
        GetState(ref state_Idle);
        GetState(ref state_Draw);
        GetState(ref state_Shoot);
        SetDefaultState(state_Idle, EMPTY_STATE_ACTION);
    }
    protected override void InitBehaviours()
    {
        bvr_Idle = new Single(
            state_Idle,
            EMPTY_STATE_ACTION,
            () => false);

        bvr_Shoot = new Sequence(
            (state_Draw,
            EMPTY_STATE_ACTION,
            () => PlayerWeaponKeys.GetKeyUp(PlayerWeaponKeys.MainAbility)),
            (state_Shoot,
            EMPTY_STATE_ACTION,
            () => weaponItem.Timer_Shoot.IsEnded));
    }
    protected override void InitObjectives()
    {
        // Pause
        NewObjective(
            () => !weaponItem.IsSelected || PlayerStatus.Incapacitated, true)
            .AddBehaviour(bvr_Idle);

        // Draw & Shoot
        NewObjective(
            () => PlayerWeaponKeys.GetKey(PlayerWeaponKeys.MainAbility))
            .AddBehaviour(bvr_Shoot, true);

        // Default
        SetDefaultObjective()
            .AddBehaviour(bvr_Idle);
    }
}
