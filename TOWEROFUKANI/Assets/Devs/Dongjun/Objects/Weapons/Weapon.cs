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

    #region Var: Properties
    public bool IsSelected { get; protected set; } = false;
    public GameObject SpriteRoot => spriteRoot;
    #endregion


    protected void Awake()
    {
        transform.localPosition = new Vector2(transform.localPosition.x, pivotPointY);
        SpriteRoot.SetActive(false);
    }

    public void SelectWeapon(bool select)
    {
        IsSelected = select;
        SpriteRoot.SetActive(select);
    }
}
