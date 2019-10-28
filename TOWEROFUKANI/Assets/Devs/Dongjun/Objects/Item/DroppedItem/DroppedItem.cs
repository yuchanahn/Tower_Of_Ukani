using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    [SerializeField] private Item item;
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
                WeaponItem weapon = Instantiate(item.gameObject, GM.PlayerObj.transform).GetComponent<WeaponItem>();
                WeaponHolder.Inst.AddWeapon(weapon);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
