using UnityEngine;

public abstract class UpgradableItem : Item
{
    #region Var: Properties
    public int ItemLevel
    { get; private set; } = 1;
    #endregion

    #region Method: Unity
    protected virtual void Start()
    {
        InitStats();
    }
    #endregion

    #region Method: Initialize
    public abstract void InitStats();
    #endregion

    #region Method: Add Level
    public void AddLevel(int amount = 1)
    {
        ItemLevel += amount;
    }
    #endregion
}
