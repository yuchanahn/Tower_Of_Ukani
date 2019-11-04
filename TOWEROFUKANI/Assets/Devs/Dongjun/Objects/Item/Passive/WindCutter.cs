using UnityEngine;

public class WindCutter : PassiveItem
{
    #region Var: Inspector
    [SerializeField] private string debugText = "바람을 가른다!";
    [SerializeField] private int bonusReloadSpeed;
    #endregion

    #region Var: Effects
    protected ItemEffect onJumpEffect = new ItemEffect();
    #endregion

    #region Method: Unity
    private void Awake()
    {
        onJumpEffect.action = OnJump;
    }
    #endregion

    #region Method: Item Effect
    private void OnJump()
    {
        Debug.Log(debugText);
    }
    #endregion

    #region On Inventory Add / Remove
    public override void OnAdd()
    {
        if (Inventory.PassiveItemSlot.Add(this))
        {
            ApplyStats();
            ItemEffectManager.AddEffect(PlayerActions.Jump, onJumpEffect);
        }
    }
    public override void OnRemove()
    {
        ItemEffectManager.RemoveEffect(PlayerActions.Jump, onJumpEffect);
    }

    private void ApplyStats()
    {
        //GunItem item;

        //for (int i = 0; i < Inventory.Inst.slot_Item.Count; i++)
        //{
        //    if (Inventory.Inst.slot_Item[i] is GunItem)
        //    {
        //        item = Inventory.Inst.slot_Item[i] as GunItem;
        //        item.reloadTimer.EndTime.flatBonus += bonusReloadSpeed;
        //    }
        //}
    }
    #endregion
}
