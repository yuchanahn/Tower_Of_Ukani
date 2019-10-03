using UnityEngine;

public enum WeaponType
{
    Physical,
    Magical,
    Living
}
public enum WeaponRange
{
    Melee,
    Ranged
}

public abstract class Weapon : CLA_Main
{
    #region Var: Inspector
    [Header("Weapon Info")]
    [SerializeField] private string weaponName = "Weapon";
    [SerializeField] private string weaponDesc = "This is a Weapon";
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private WeaponRange weaponRange;

    [Header("Visuals")]
    [SerializeField] private float pivotPointY;
    [SerializeField] private GameObject spriteRoot;
    #endregion

    #region Var: Weapon Info
    private string weaponNameTrimed;
    #endregion

    #region Var: Properties
    public bool IsSelected { get; protected set; } = false;

    public string WeaponName => weaponName;
    public string WeaponNameTrimed => weaponNameTrimed;
    public string WeaponDesc => weaponDesc;
    public WeaponType WeaponType => weaponType;
    public WeaponRange WeaponRange => weaponRange;

    public GameObject SpriteRoot => spriteRoot;
    #endregion


    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        weaponNameTrimed = weaponName.Replace(" ", string.Empty);

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
