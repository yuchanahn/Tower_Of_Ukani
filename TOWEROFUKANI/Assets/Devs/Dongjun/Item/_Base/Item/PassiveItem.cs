using UnityEngine;

public abstract class PassiveItem : UpgradableItem
{
    #region Var: Inspector
    [Header("God")]
    [SerializeField] private TowerOfUkani.Gods god;
    #endregion

    #region Var: Properties
    public TowerOfUkani.Gods God => god;
    #endregion

    #region Method: Bonus Stats
    public abstract void ApplyBonusStats(WeaponItem weapon);
    #endregion
}
