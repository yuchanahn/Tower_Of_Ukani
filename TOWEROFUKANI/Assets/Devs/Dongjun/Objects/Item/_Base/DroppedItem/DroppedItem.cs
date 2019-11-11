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
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    #endregion

    #region Method: Dropped Item
    public void InitRef_Item(Item item)
    {
        this.item = item;
    }

    public abstract void OnPickUp();
    public virtual void Activate()
    {
        transform.position = GM.PlayerPos;
        gameObject.SetActive(true);
    }
    #endregion
}
