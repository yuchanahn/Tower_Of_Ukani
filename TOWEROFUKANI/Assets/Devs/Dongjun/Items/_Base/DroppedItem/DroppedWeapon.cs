using UnityEngine;

public class DroppedWeapon : DroppedItem
{

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        // Initialize Icon
        spriteRenderer.sprite = Item.Info.Icon;
    }
    #endregion

    #region Method Override: On Pick Up
    public override void OnPickUp()
    {
        WeaponItem weaponItem;

        if (!DroppedFromInventory)
        {
            weaponItem = Instantiate(Item.gameObject).GetComponent<WeaponItem>();
            if (WeaponHotbar.AddExisting(weaponItem))
            {
                Destroy(weaponItem.gameObject);
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            weaponItem = Item as WeaponItem;
        }

        // Add to Inventory
        if (WeaponHotbar.Add(weaponItem))
        {
            weaponItem.gameObject.SetActive(true);
            weaponItem.transform.SetParent(GM.PlayerObj.transform);
            weaponItem.transform.localPosition = new Vector2(0, weaponItem.PivotPointY);
            Destroy(gameObject);
        }
        else if (Inventory.Add(weaponItem))
        {
            Destroy(gameObject);
        }
    }
    #endregion
}
