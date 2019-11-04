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
        spriteRenderer.sprite = item.Info.Icon;
    }
    #endregion

    #region Method: Dropped Item
    public override void OnPickUp()
    {
        if (weaponItem is null)
        {
            weaponItem = Instantiate(item.gameObject, GM.PlayerObj.transform).GetComponent<WeaponItem>();
            weaponItem.Init(this);
        }

        if (Inventory.WeaponHotbar.Add(weaponItem))
        {
            gameObject.SetActive(false);

            weaponItem.gameObject.SetActive(true);
            weaponItem.transform.SetParent(GM.PlayerObj.transform);
            weaponItem.transform.localPosition = new Vector2(0, weaponItem.PivotPointY);
        }
    }
    #endregion
}
