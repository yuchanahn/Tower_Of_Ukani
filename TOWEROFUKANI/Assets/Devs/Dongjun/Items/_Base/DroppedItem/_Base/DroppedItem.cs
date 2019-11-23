using UnityEngine;

public abstract class DroppedItem : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] private Item item;
    #endregion

    #region Var: Component
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region Var: Properties
    public Item Item => item;
    public bool DroppedFromInventory
    { get; private set; } = false;
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    #endregion

    #region Method: Set Item Ref
    public void SetRef_Item(Item item)
    {
        this.item = item;
        DroppedFromInventory = true;
    }
    #endregion

    #region Method: On Pick Up
    public abstract void OnPickUp();
    #endregion
}
