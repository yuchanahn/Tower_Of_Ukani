using System.Collections.Generic;
using UnityEngine;

public enum WeaponTag
{
    Physical,
    Magical,
    Living,

    Ranged,
    Melee,

    Gun,
    Bow,
}

public abstract class Weapon : CLA_Main
{
    #region Var: Inspector
    [Header("Weapon Info")]
    [SerializeField] private Sprite weaponIcon;
    [SerializeField] private string weaponName = "Weapon";
    [SerializeField] private string weaponDesc = "This is a Weapon";
    [SerializeField] private WeaponTag[] weaponTags;

    [Header("Visuals")]
    [SerializeField] private float pivotPointY;
    [SerializeField] private GameObject spriteRoot;
    #endregion

    #region Var: Properties
    public bool IsSelected { get; protected set; } = false;

    public Sprite WeaponIcon => weaponIcon;
    public string WeaponName => weaponName;
    public string WeaponNameTrimed { get; private set; }
    public string WeaponDesc => weaponDesc;
    public HashSet<WeaponTag> WeaponTags { get; private set; } = new HashSet<WeaponTag>();

    public GameObject SpriteRoot => spriteRoot;
    #endregion


    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        WeaponNameTrimed = weaponName.Replace(" ", string.Empty);

        if (weaponTags != null)
        {
            for (int i = 0; i < weaponTags.Length; i++)
                WeaponTags.Add(weaponTags[i]);
        }

        transform.localPosition = new Vector2(transform.localPosition.x, pivotPointY);
    }
    protected override void Start()
    {
        base.Start();

        SpriteRoot.SetActive(IsSelected);
    }
    #endregion

    #region Method: Select Weapon
    public void SelectWeapon(bool select)
    {
        IsSelected = select;
        SpriteRoot.SetActive(select);
    }
    #endregion
}
