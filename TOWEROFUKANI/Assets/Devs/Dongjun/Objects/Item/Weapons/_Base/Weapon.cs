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
    public HashSet<WeaponTag> WeaponTags 
    { get; private set; } = new HashSet<WeaponTag>();
    public bool IsSelected 
    { get; protected set; } = false;


    protected override void Awake()
    {
        base.Awake();

        if (weaponTags != null)
        {
            for (int i = 0; i < weaponTags.Length; i++)
                WeaponTags.Add(weaponTags[i]);
        }
    }

    public override void OnAdd()
    {
        SelectWeapon(false);
        transform.localPosition = new Vector2(transform.localPosition.x, pivotPointY);
    }
    public override void OnRemove()
    {
        IsSelected = false;
        spriteRoot.SetActive(true);
    }

    public void SelectWeapon(bool select)
    {
        IsSelected = select;
        spriteRoot.SetActive(select);
    }
}

public abstract class WeaponObject_Base : CLA_Main { }
public abstract class WeaponObject<TItem> : WeaponObject_Base
    where TItem : WeaponItem
{

    protected TItem weaponItem;

    protected override void Awake()
    {
        base.Awake();
        weaponItem = GetComponent<WeaponItem>() as TItem;
    }
}