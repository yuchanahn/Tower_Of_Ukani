using UnityEngine;

public class Fist : MeleeController<FistItem>
{
    private Fist_Basic state_Basic;
    private Fist_Heavy state_Heavy;
    private Fist_Slam state_Slam;
    private Fist_Dash state_Dash;

    protected override void InitStates()
    {
        SetLogic(ref state_Basic, () =>
        {
            if (Input.GetKey(PlayerWeaponKeys.SubAbility))
                return state_Heavy;

            return null;
        });

        SetLogic(ref state_Heavy, () =>
        {
            if (state_Heavy.PunchAnimEnd)
                return state_Basic;

            return null;
        });

        SetLogic(ref state_Slam, () =>
        {
            return null;
        });

        SetLogic(ref state_Dash, () =>
        {
            if (!GM.Player.IsDashing)
                return state_Basic;

            return null;
        });

        SetLogic(When.AnyAction, () =>
        {
            if (GM.Player.IsDashing)
                return state_Dash;

            return null;
        });

        SetDefaultState(state_Basic);
    }
}
