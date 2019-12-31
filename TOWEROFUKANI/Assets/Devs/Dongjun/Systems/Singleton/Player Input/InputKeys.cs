using UnityEngine;

public static class PlayerMovementKeys
{
    public static KeyCode WalkRight => KeyCode.D;
    public static KeyCode WalkLeft => KeyCode.A;
    public static KeyCode FallThrough => KeyCode.S;
    public static KeyCode Jump => KeyCode.Space;
}
public static class PlayerActionKeys
{
    public static KeyCode Dash => KeyCode.LeftShift;
    public static KeyCode Kick => KeyCode.LeftControl;
    public static KeyCode PickUpItem => KeyCode.C;
    public static KeyCode DropItem => KeyCode.Q;
}
public static class PlayerWeaponKeys
{
    public static KeyCode MainAbility => KeyCode.Mouse0;
    public static KeyCode SubAbility => KeyCode.Mouse1;
    public static KeyCode SpecialAbility => KeyCode.F;
    public static KeyCode Reload => KeyCode.R;
}

public static class PlayerUIKeys
{
    public static KeyCode InventoryToggle => KeyCode.E;
    public static KeyCode PassiveInventoryToggle => KeyCode.Tab;
    public static KeyCode Escape => KeyCode.R;
}
