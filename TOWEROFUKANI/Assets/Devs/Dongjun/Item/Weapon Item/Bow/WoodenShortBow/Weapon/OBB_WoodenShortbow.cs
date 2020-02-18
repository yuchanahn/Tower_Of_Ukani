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
    private Single bvr_Draw;
    private Single bvr_Shoot;

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

        bvr_Draw = new Single(
            state_Draw,
            EMPTY_STATE_ACTION,
            () => PlayerWeaponKeys.GetKeyUp(PlayerWeaponKeys.MainAbility));

        bvr_Shoot = new Single(
            state_Shoot,
            EMPTY_STATE_ACTION,
            () => weaponItem.Timer_Shoot.IsEnded);
    }
    protected override void InitObjectives()
    {
        // Pause
        NewObjective(
            () => !weaponItem.IsSelected || PlayerStatus.Incapacitated, true)
            .AddBehaviour(bvr_Idle);

        // Normal
        NewObjective(
            () => PlayerWeaponKeys.GetKey(PlayerWeaponKeys.MainAbility))
            .AddBehaviour(bvr_Draw, true)
            .SetTransition(
                () => true)
                .AddBehaviour(bvr_Shoot);

        // Default
        SetDefaultObjective()
            .AddBehaviour(bvr_Idle);
    }
}
