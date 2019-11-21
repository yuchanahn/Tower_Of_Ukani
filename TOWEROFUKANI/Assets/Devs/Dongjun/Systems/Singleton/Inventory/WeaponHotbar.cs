using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponHotbar : SingletonBase<WeaponHotbar>
{
    #region Var: Const
    public const int SLOT_SIZE = 3;
    #endregion

    #region Var: Inspector
    [Header("Sprite")]
    [SerializeField] private Sprite spr_Empty;

    [Header("Weapon Slot")]
    [SerializeField] private Image[] img_WeaponSlots = new Image[SLOT_SIZE];
    [SerializeField] private GameObject[] go_WeaponSelected = new GameObject[SLOT_SIZE];

    [Header("Weapon Info")]
    [SerializeField] private Text txt_WeaponName;
    [SerializeField] private Text txt_AmmoInfo;
    #endregion

    #region Var: Properties
    public static WeaponItem[] Weapons
    { get; private set; } = new WeaponItem[SLOT_SIZE];
    public static int EmptySlotCount
    { get; private set; } = SLOT_SIZE;
    public static bool IsFull => EmptySlotCount == 0;

    public static int Index_Cur
    { get; private set; } = 0;
    public static int Index_Prev
    { get; private set; } = 0;
    public static WeaponItem CurSelected => Weapons[Index_Cur];
    public static WeaponItem PrevSelected => Weapons[Index_Prev];
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        Clear();
        InitUI_WeaopnHotbar();
    }
    private void LateUpdate()
    {
        UpdateUI_SelectedWeapon();
        UpdateUI_WeaponInfo();
    }
    #endregion

    #region Method: UI
    private void InitUI_WeaopnHotbar()
    {
        go_WeaponSelected[Index_Cur].SetActive(true);

        txt_WeaponName.text = "None";
        txt_AmmoInfo.text = string.Empty;
    }
    private void UpdateUI_WeaponHotbar()
    {
        for (int i = 0; i < 3; i++)
        {
            img_WeaponSlots[i].sprite = Weapons[i]?.Info.Icon ?? spr_Empty;
        }
    }
    private void UpdateUI_WeaponInfo()
    {
        // Update Weapon Name Text
        txt_WeaponName.text = CurSelected?.Info.Name ?? "None";

        // Update Weapon Specific Info Text
        switch (CurSelected)
        {
            case GunItem gun:
                txt_AmmoInfo.text = $"{gun.loadedBullets} / {gun.magazineSize.Value}";
                break;
            default:
                txt_AmmoInfo.text = string.Empty;
                break;
        }
    }
    private void UpdateUI_SelectedWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            ScrollUp();

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            if (scroll < 0)
                ScrollUp();
            else
                ScrollDown();
        }

        if (Index_Prev != Index_Cur)
        {
            go_WeaponSelected[Index_Prev].SetActive(false);
            go_WeaponSelected[Index_Cur].SetActive(true);
            UpdateIndex();
        }
    }
    #endregion

    #region Method: Add/Remove
    private static void AddWeapon(int index, WeaponItem weapon)
    {
        EmptySlotCount--;
        Weapons[index] = weapon;
        weapon.OnAdd();
        PassiveInventory.ApplyAllBonusStats();
        Inst.UpdateUI_WeaponHotbar();
    }
    public static bool Add(WeaponItem weapon)
    {
        if (IsFull) return false;

        if (Weapons[Index_Cur] is null)
        {
            AddWeapon(Index_Cur, weapon);
            return true;
        }

        int emptyIndex = Array.IndexOf(Weapons, default);
        if (emptyIndex != -1)
        {
            AddWeapon(emptyIndex, weapon);
            return true;
        }

        return false;
    }
    public static void Remove()
    {
        if (Weapons[Index_Cur] is null)
            return;

        Weapons[Index_Cur].OnRemove();
        Weapons[Index_Cur] = null;
        EmptySlotCount++;
        Inst.UpdateUI_WeaponHotbar();
    }
    public static void Clear()
    {
        Weapons = new WeaponItem[SLOT_SIZE];
        EmptySlotCount = SLOT_SIZE;
    }
    #endregion

    #region Method: Change Weapon
    public static void UpdateIndex() => Index_Prev = Index_Cur;
    public static void ScrollUp()
    {
        Index_Cur += Index_Cur == 2 ? -2 : 1;
        PrevSelected?.Select(false);
        CurSelected?.Select(true);
    }
    public static void ScrollDown()
    {
        Index_Cur -= Index_Cur == 0 ? -2 : 1;
        PrevSelected?.Select(false);
        CurSelected?.Select(true);
    }
    #endregion
}
