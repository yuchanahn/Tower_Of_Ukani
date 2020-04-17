using UnityEngine;

public abstract class UpgradableItem : Item
{
    #region Prop: 
    public int ItemLevel
    { get; private set; } = 1;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        InitStats();
    }
    #endregion

    #region Method: Initialize
    public abstract void InitStats();
    #endregion

    #region Method: Add Level
    public virtual void AddLevel(int amount = 1)
    {
        ItemLevel += amount;
        InitStats();
    }
    #endregion
}
