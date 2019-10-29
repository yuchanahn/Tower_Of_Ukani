using System.Collections.Generic;
using UnityEngine;

public enum WeaponTag
{
    Gun,
    Bow,
}

public abstract class WeaponItem : Item 
{
    [SerializeField] private GameObject spriteRoot;
    [SerializeField] private float pivotPointY;
    [SerializeField] private WeaponTag[] weaponTags;

    public GameObject SpriteRoot => spriteRoot;
    public float PivotPointY => pivotPointY;
    public HashSet<WeaponTag> WeaponTags { get; private set; } = new HashSet<WeaponTag>();
    public bool IsSelected { get; protected set; } = false;


    protected override void Awake()
    {
        base.Awake();

        // Init Weapon Tags
        if (weaponTags != null)
        {
            for (int i = 0; i < weaponTags.Length; i++)
                WeaponTags.Add(weaponTags[i]);
        }
    }

    public override void OnAdd()
    {
        SelectWeapon(false);
    }
    public override void OnRemove()
    {
        IsSelected = false;
        spriteRoot.SetActive(true);

        // Detactivate Self
        gameObject.SetActive(false);
        transform.SetParent(null);

        // Activate Dropped Item
        droppedItem.Init(this);
        droppedItem.transform.position = GM.PlayerPos;
        droppedItem.gameObject.SetActive(true);
    }

    public void SelectWeapon(bool select)
    {
        IsSelected = select;
        spriteRoot.SetActive(select);
    }
}

public abstract class WeaponController_Base : CLA_Main { }
public abstract class WeaponController<TItem> : WeaponController_Base
    where TItem : WeaponItem
{
    protected TItem weaponItem;

    protected override void Awake()
    {
        base.Awake();
        weaponItem = GetComponent<WeaponItem>() as TItem;
    }
}