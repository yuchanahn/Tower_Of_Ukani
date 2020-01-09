using UnityEngine;

public class SlotUI : MonoBehaviour
{
    #region Var: Ref
    protected InventoryUIBase inventoryUI;
    #endregion

    #region Var: Properties
    public int Index
    { get; private set; }
    #endregion

    #region Method: Init
    public virtual void Init(InventoryUIBase inventoryUI, int index)
    {
        this.inventoryUI = inventoryUI;
        Index = index;
    }
    #endregion
}
