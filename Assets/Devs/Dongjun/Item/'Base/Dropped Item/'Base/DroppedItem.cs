using DG.Tweening;
using UnityEngine;

public abstract class DroppedItem : MonoBehaviour
{
    #region Var: Inspector
    [SerializeField] private Item item;
    #endregion

    #region Var: Component
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region Prop: 
    public Item Item => item;
    #endregion

    #region Method: Unity
    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Initialize Icon
        spriteRenderer.sprite = Item.Info.Icon;
    }
    protected virtual void Start()
    {
        spriteRenderer.transform.DOLocalMoveY(0.3f, 0.35f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    #endregion

    #region Method: Set Item Ref
    public void SetRef_Item(Item item)
    {
        this.item = item;
    }
    #endregion

    #region Method: On Pick Up
    public abstract void OnPickUp(PlayerItemPickUpData data);
    #endregion
}
