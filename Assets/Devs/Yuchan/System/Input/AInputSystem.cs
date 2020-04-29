using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum eKeyID
{
    WalkLeft,
    WalkRight,
    Jump,
    FallThrough,
    Dash,
    Kick,
    PickUpItem,
    DropWeapon,
    Interact,
    MainAbility,
    SubAbility,
    SpecialAbility,
    Reload,
    InventoryToggle,
    ActiveHotbarToggle,
    last
}


[System.Serializable]
public struct AINPUT
{
    public KeyCode Key;
    public eKeyID MappingTo;
}

public static class IN
{
    private static Dictionary<eKeyID, KeyCode> keys = new Dictionary<eKeyID, KeyCode>();

    public static System.Action PrevSetEvent = () => { };
    public static System.Action SetEvent = () => { };

    public static Dictionary<eKeyID, KeyCode> Keys { get => keys; set { PrevSetEvent(); keys = value; SetEvent(); } }

    public static bool Down(this eKeyID k) => Input.GetKeyDown(Keys[k]);
    public static bool Up(this eKeyID k) => Input.GetKeyUp(Keys[k]);
    public static bool Get(this eKeyID k) => Input.GetKey(Keys[k]);
}

public class AInputSystem : MonoBehaviour
{
    static AInputSystem Inst;

    [SerializeField] AINPUT[] DefaultKeys = null;

    public void set_key_bind_defult()
    {
        DefaultKeys = new AINPUT[(int)eKeyID.last];
        DefaultKeys[(int)eKeyID.WalkLeft].Key = KeyCode.A;
        DefaultKeys[(int)eKeyID.WalkRight].Key = KeyCode.D;
        DefaultKeys[(int)eKeyID.Jump].Key = KeyCode.Space;
        DefaultKeys[(int)eKeyID.FallThrough].Key = KeyCode.S;
        DefaultKeys[(int)eKeyID.Dash].Key = KeyCode.LeftShift;
        DefaultKeys[(int)eKeyID.Kick].Key = KeyCode.LeftControl;
        DefaultKeys[(int)eKeyID.PickUpItem].Key = KeyCode.C;
        DefaultKeys[(int)eKeyID.DropWeapon].Key = KeyCode.V;
        DefaultKeys[(int)eKeyID.Interact].Key = KeyCode.Z;
        DefaultKeys[(int)eKeyID.MainAbility].Key = KeyCode.Mouse0;
        DefaultKeys[(int)eKeyID.SubAbility].Key = KeyCode.Mouse1;
        DefaultKeys[(int)eKeyID.SpecialAbility].Key = KeyCode.F;
        DefaultKeys[(int)eKeyID.Reload].Key = KeyCode.R;
        DefaultKeys[(int)eKeyID.InventoryToggle].Key = KeyCode.E;
        DefaultKeys[(int)eKeyID.ActiveHotbarToggle].Key = KeyCode.Q;

        DefaultKeys[(int)eKeyID.WalkLeft].MappingTo = eKeyID.WalkLeft;
        DefaultKeys[(int)eKeyID.WalkRight].MappingTo = eKeyID.WalkRight;
        DefaultKeys[(int)eKeyID.Jump].MappingTo = eKeyID.Jump;
        DefaultKeys[(int)eKeyID.FallThrough].MappingTo = eKeyID.FallThrough;
        DefaultKeys[(int)eKeyID.Dash].MappingTo = eKeyID.Dash;
        DefaultKeys[(int)eKeyID.Kick].MappingTo = eKeyID.Kick;
        DefaultKeys[(int)eKeyID.PickUpItem].MappingTo = eKeyID.PickUpItem;
        DefaultKeys[(int)eKeyID.DropWeapon].MappingTo = eKeyID.DropWeapon;
        DefaultKeys[(int)eKeyID.Interact].MappingTo = eKeyID.Interact;
        DefaultKeys[(int)eKeyID.MainAbility].MappingTo = eKeyID.MainAbility;
        DefaultKeys[(int)eKeyID.SubAbility].MappingTo = eKeyID.SubAbility;
        DefaultKeys[(int)eKeyID.SpecialAbility].MappingTo = eKeyID.SpecialAbility;
        DefaultKeys[(int)eKeyID.Reload].MappingTo = eKeyID.Reload;
        DefaultKeys[(int)eKeyID.InventoryToggle].MappingTo = eKeyID.InventoryToggle;
        DefaultKeys[(int)eKeyID.ActiveHotbarToggle].MappingTo = eKeyID.ActiveHotbarToggle;
    }
    public void key_mapping() => IN.Keys = DefaultKeys.ToDictionary(i => i.MappingTo, i => i.Key);

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        Inst.set_key_bind_defult();
        Inst.key_mapping();
    }

    private void Update()
    {
    }
}
