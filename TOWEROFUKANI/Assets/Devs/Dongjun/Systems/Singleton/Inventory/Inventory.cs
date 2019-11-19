using System;
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
    protected override void Awake()
    {
        base.Awake();

        ItemSlot.Clear();
        WeaponHotbar.Clear();
        ActiveItemHotbar.Clear();
        PassiveItemSlot.Clear();

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
                if (Items[i] != null)
                    continue;

                empySlotCount--;
                Items[i] = item;
                Items[i].OnAdd();

                switch (item)
                {
                    case WeaponItem weapon:
                        PassiveItemSlot.ApplyBonusStats();
                        WeaponHotbar.Add(weapon);
                        break;
                    //case ActiveItem activeItem:
                    //    ActiveItemHotbar.Add(activeItem);
                    //break;
                    default:
                        break;
                }

                return true;
            }

            return false;
        }
        public static bool Remove(Item item)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] == item)
                {
                    empySlotCount++;
                    Items[i].OnRemove();
                    Items[i] = null;


                    return true;
                }
            }

            return false;
        }
        public static void Clear()
        {
            Items = new Item[15];
            empySlotCount = Items.Length;
        }
        public static bool IsFull()
        {
            return empySlotCount == 0;
        }

        public static void ResetAllWeaponStats()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] is null) continue;

                if (Items[i] is WeaponItem)
                    (Items[i] as WeaponItem).InitStats();
            }
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
            empySlotCount--;
            Weapons[index] = weapon;
            weapon.OnAdd();

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
                if (Weapons[i] != null)
                    continue;

                AddWeapon(i, weapon);
                return true;
            }

            return false;
        }
        public static void Remove()
        {
            if (Weapons[index_Cur] is null)
                return;

            empySlotCount++;
            ItemSlot.Remove(Weapons[index_Cur]);
            Weapons[index_Cur] = null;

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

            empySlotCount--;
            Items[index] = item;
            item.OnAdd();
        }
        public static void Remove(int index)
        {
            if (Items[index] is null)
                return;

            empySlotCount++;
            Items[index].OnRemove();
            Items[index] = null;
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
        public static Dictionary<Type, PassiveItem> Items
        { get; private set; } = new Dictionary<Type, PassiveItem>();

        public static bool Add(PassiveItem item)
        {
            if (!Items.ContainsKey(item.GetType()))
            {
                Items.Add(item.GetType(), item);
                ApplyBonusStats();
                return true;
            }

            item = Items[item.GetType()];
            if (item.Cur_Count < item.Max_Count)
            {
                item.Cur_Count++;
                ApplyBonusStats();
                return true;
            }

            return false;
        }
        public static void Clear()
        {
            Items = new Dictionary<Type, PassiveItem>();
        }

        public static void ApplyBonusStats()
        {
            // Reset All Weapon Stats
            ItemSlot.ResetAllWeaponStats();

            // Apply Bonus Stats
            foreach (KeyValuePair<Type, PassiveItem> item in Items)
            {
                item.Value.ApplyBonusStats();
            }
        }
    }
    #endregion
}