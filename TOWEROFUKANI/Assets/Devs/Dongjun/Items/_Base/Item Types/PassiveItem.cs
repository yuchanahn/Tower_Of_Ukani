using UnityEngine;

public abstract class PassiveItem : Item
{
    #region Var: Inspector
    [Header("Affected Weapon")]
    [SerializeField] private WeaponItem.WeaponTag[] affectedWeapons;

    [Header("God")]
    [SerializeField] private TowerOfUkani.Gods god;
    #endregion

    #region Var: Properties
    public WeaponItem.WeaponTag[] AffectedWeapons => affectedWeapons;
    public TowerOfUkani.Gods God => god;
    #endregion

    #region Method: Set Bonus Stats
    protected abstract void SetBonusStats(WeaponItem weapon);
    #endregion

    #region Method: Apply Bonus Stats
    public void ApplyBonusStats()
    {
        // Weapon Hotbar
        for (int i = 0; i < WeaponHotbar.SLOT_SIZE; i++)
        {
            if (WeaponHotbar.Weapons[i] == null)
                continue;

            SetBonusStats(WeaponHotbar.Weapons[i]);
        }

        // Inventory
        for (int i = 0; i < Inventory.Items.Length; i++)
        {
            if (Inventory.Items[i] == null)
                continue;

            if (Inventory.Items[i] is WeaponItem)
                SetBonusStats(Inventory.Items[i] as WeaponItem);
        }
    }
    #endregion
}
