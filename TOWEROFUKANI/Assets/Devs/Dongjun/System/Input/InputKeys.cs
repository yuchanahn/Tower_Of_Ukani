using UnityEngine;

public static class PlayerMovementKeys
{
    public static KeyCode WalkLeft => KeyCode.A;
    public static KeyCode WalkRight => KeyCode.D;
    public static KeyCode Jump => KeyCode.Space;
    public static KeyCode FallThrough => KeyCode.S;
    public static KeyCode Dash => KeyCode.LeftShift;
}
public static class PlayerActionKeys
{
    public static KeyCode Kick => KeyCode.LeftControl;
    public static KeyCode PickUpItem => KeyCode.C;
    public static KeyCode DropWeapon => KeyCode.V;
}
public static class PlayerWeaponKeys
{
    public static KeyCode MainAbility => KeyCode.Mouse0;
    public static KeyCode SubAbility => KeyCode.Mouse1;
    public static KeyCode SpecialAbility => KeyCode.E;
    public static KeyCode Reload => KeyCode.R;

    public static bool GetKey(KeyCode keyCode)
    {
        return PlayerInputManager.Inst.CanUseWeapon && Input.GetKey(keyCode);
    }
    public static bool GetKeyDown(KeyCode keyCode)
    {
        return PlayerInputManager.Inst.CanUseWeapon && Input.GetKeyDown(keyCode);
    }
    public static bool GetKeyUp(KeyCode keyCode)
    {
        return PlayerInputManager.Inst.CanUseWeapon && Input.GetKeyUp(keyCode);
    }
}

public static class PlayerUIKeys
{
    public static KeyCode InventoryToggle => KeyCode.E;
    public static KeyCode ActiveHotbarToggle => KeyCode.Q;
}
