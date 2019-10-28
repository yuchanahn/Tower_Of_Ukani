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
    protected override void Awake()
    {
        base.Awake();
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
        if (Inventory.Inst.HasItem(this))
        {
            // Upgrade This Item
        }
        else
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
        GunItem item;

        for (int i = 0; i < Inventory.Inst.items.Count; i++)
        {
            if (Inventory.Inst.items[i] is GunItem)
            {
                item = Inventory.Inst.items[i] as GunItem;
                item.reloadTimer.EndTime.flatBonus += bonusReloadSpeed;
            }
        }
    }
    #endregion
}
