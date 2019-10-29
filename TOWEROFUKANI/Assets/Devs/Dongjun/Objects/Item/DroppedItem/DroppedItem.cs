using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private Item item;

    private WeaponItem weapon;
    private SpriteRenderer spriteRenderer;

    public Item Item => item;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = item.Info.Icon;
    }

    public void Init(Item item)
    {
        this.item = item;
    }
    public void OnPickUp()
    {
        Inventory.Inst.AddItem(item);

        if (item is WeaponItem)
        {
            if (WeaponHolder.Inst.HotbarAvailable())
            {
                if (weapon is null)
                {
                    weapon = Instantiate(item.gameObject, GM.PlayerObj.transform).GetComponent<WeaponItem>();
                    weapon.Init(this);
                }
                else
                {
                    weapon.gameObject.SetActive(true);
                }

                weapon.transform.SetParent(GM.PlayerObj.transform);
                weapon.transform.localPosition = new Vector2(0, weapon.PivotPointY);
                WeaponHolder.Inst.AddWeapon(weapon);
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
