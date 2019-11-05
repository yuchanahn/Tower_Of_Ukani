using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : SingletonBase<Inventory>
{
    #region Var: Inspector
    [Header("Sprite")]
    [SerializeField] private Sprite spr_Empty;

    [Header("Weapon Slot")]
    [SerializeField] private Image[] img_WeaponSlots = new Image[3];
    [SerializeField] private GameObject[] go_WeaponSelected = new GameObject[3];

    [Header("Weapon Info")]
    [SerializeField] private Text txt_WeaponName;
    [SerializeField] private Text txt_AmmoInfo;
    #endregion

    #region Method: Unity
    private void Awake()
    {
        WeaponHotbar.Clear();
        ActiveItemHotbar.Clear();

        InitUI_WeaopnHotbar();
    }
    private void LateUpdate()
    {
        UpdateSelectedWeapon();
        UpdateUI_WeaponInfo();
    }
    #endregion

    #region Method: Weapon Hotbar
    private void InitUI_WeaopnHotbar()
    {
        go_WeaponSelected[WeaponHotbar.index_Cur].SetActive(true);

        txt_WeaponName.text = "None";
        txt_AmmoInfo.text = string.Empty;
    }
    private void UpdateUI_WeaponHotbar()
    {
        for (int i = 0; i < 3; i++)
        {
            img_WeaponSlots[i].sprite = WeaponHotbar.Weapons[i]?.Info.Icon ?? spr_Empty;
        }
    }
    private void UpdateUI_WeaponInfo()
    {
        txt_WeaponName.text = WeaponHotbar.GetSelected()?.Info.Name ?? "None";

        switch (WeaponHotbar.GetSelected())
        {
            case GunItem gun:
                txt_AmmoInfo.text = $"{gun.loadedBullets} / {gun.magazineSize.Value}";
                break;
            default:
                txt_AmmoInfo.text = string.Empty;
                break;
        }
    }
    public void UpdateSelectedWeapon()
    {
        WeaponHotbar.index_Old = WeaponHotbar.index_Cur;

        if (Input.GetKeyDown(KeyCode.Tab))
            WeaponHotbar.index_Cur += WeaponHotbar.index_Cur == 2 ? -2 : 1;

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            if (scroll < 0)
                WeaponHotbar.index_Cur += WeaponHotbar.index_Cur == 2 ? -2 : 1;
            else
                WeaponHotbar.index_Cur -= WeaponHotbar.index_Cur == 0 ? -2 : 1;
        }

        if (WeaponHotbar.index_Old != WeaponHotbar.index_Cur)
        {
            WeaponHotbar.GetOld()?.Select(false);
            WeaponHotbar.GetSelected()?.Select(true);

            // Update UI
            Inst.go_WeaponSelected[WeaponHotbar.index_Old].SetActive(false);
            Inst.go_WeaponSelected[WeaponHotbar.index_Cur].SetActive(true);
        }
    }
    #endregion

    #region Class: Item Slot
    public static class ItemSlot
    {
        public static Item[] Items { get; private set; } = new Item[15];
        public static int empySlotCount { get; private set; } = Items.Length;

        public static bool Add(Item item)
        {
            if (IsFull()) return false;

            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] is null)
                {
                    Items[i] = item;
                    item.OnAdd();
                    empySlotCount--;
                    return true;
                }
            }

            return false;
        }
        public static bool Remove(Item item)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] == item)
                {
                    Items[i].OnRemove();
                    Items[i] = null;
                    empySlotCount++;
                    return true;
                }
            }

            return false;
        }
        public static bool IsFull()
        {
            return empySlotCount == 0;
        }
    }
    #endregion

    #region Class: Weapon Hotbar
    public static class WeaponHotbar
    {
        public static WeaponItem[] Weapons { get; private set; } = new WeaponItem[3];
        public static int empySlotCount { get; private set; } = Weapons.Length;
        public static int index_Cur = 0;
        public static int index_Old = 0;

        private static void AddWeapon(int index, WeaponItem weapon)
        {
            Weapons[index] = weapon;
            weapon.OnAdd();
            empySlotCount--;

            // Update UI
            Inst.UpdateUI_WeaponHotbar();
        }
        public static bool Add(WeaponItem weapon)
        {
            if (IsFull()) return false;

            if (Weapons[index_Cur] is null)
            {
                AddWeapon(index_Cur, weapon);
                return true;
            }

            for (int i = 0; i < Weapons.Length; i++)
            {
                if (Weapons[i] is null)
                {
                    AddWeapon(i, weapon);
                    return true;
                }
            }

            return false;
        }
        public static void Remove()
        {
            if (Weapons[index_Cur] is null)
                return;

            Weapons[index_Cur].OnRemove();
            Weapons[index_Cur] = null;
            empySlotCount++;

            Inst.UpdateUI_WeaponHotbar();
        }
        public static void Clear()
        {
            Weapons = new WeaponItem[3];
            empySlotCount = Weapons.Length;
        }
        public static bool IsFull()
        {
            return empySlotCount == 0;
        }

        public static WeaponItem GetSelected()
        {
            return Weapons[index_Cur];
        }
        public static WeaponItem GetOld()
        {
            return Weapons[index_Old];
        }
    }
    #endregion

    #region Class: Active Item Hotbar
    public static class ActiveItemHotbar
    {
        public static ActiveItem[] Items { get; private set; } = new ActiveItem[4];
        public static int empySlotCount { get; private set; } = Items.Length;

        public static void Add(int index, ActiveItem item)
        {
            if (Items[index] != null)
                return;

            Items[index] = item;
            item.OnAdd();
            empySlotCount--;
        }
        public static void Remove(int index)
        {
            if (Items[index] is null)
                return;

            Items[index].OnRemove();
            Items[index] = null;
            empySlotCount++;
        }
        public static void Clear()
        {
            Items = new ActiveItem[4];
            empySlotCount = Items.Length;
        }
        public static bool IsFull()
        {
            return empySlotCount == 0;
        }
    }
    #endregion

    #region Class: Passive Item Slot
    public static class PassiveItemSlot
    {
        public static HashSet<PassiveItem> Items { get; private set; } = new HashSet<PassiveItem>();

        public static bool Add(PassiveItem item)
        {
            if (Items.Contains(item))
            {
                //item.Count++;
                return false;
            }

            Items.Add(item);
            return true;
        }
    }
    #endregion
}