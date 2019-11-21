using UnityEngine;

public class DroppedWeapon : DroppedItem
{
    #region Var: Weapon Item
    private WeaponItem weaponItem;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        // Initialize Icon
        spriteRenderer.sprite = Item.Info.Icon;

        // Spawn Weapon Item
        weaponItem = Instantiate(Item.gameObject).GetComponent<WeaponItem>();
        weaponItem.InitRef_DroppedItem(this);
    }
    #endregion

    #region Method: Dropped Item
    public override void OnPickUp()
    {
        if (WeaponHotbar.Add(weaponItem) || Inventory.Add(weaponItem))
        {
            gameObject.SetActive(false);
            weaponItem.gameObject.SetActive(true);
            weaponItem.transform.SetParent(GM.PlayerObj.transform);
            weaponItem.transform.localPosition = new Vector2(0, weaponItem.PivotPointY);
        }
    }
    #endregion
}
