using UnityEngine;

public class TalismanOfProtection : ActiveItem
{
    #region Var: Inspector
    [SerializeField] private GameObject shieldEffectPrefab;
    #endregion

    #region Var: Item Stats
    private TimerData durationTimer = new TimerData();

    private int shieldIndex = -1;
    private FloatStat shieldHealth;
    #endregion

    #region Var: Item Effect
    private PlayerActionEvent onShieldChanged;
    #endregion

    #region Var: Visual Effect
    private GameObject shieldEffect;
    #endregion

    #region Method: Item
    public override void InitStats()
    {
        // Init Cooldown
        CooldownTimer.EndTime = 5f;

        // Init Duration
        durationTimer.EndTime = 2.5f;
        durationTimer
            .SetTick(gameObject)
            .SetAction(onEnd: Deactivate)
            .SetActive(false);

        // Init Shield HP
        shieldHealth = new FloatStat(40, min: 0, max: 40);
    }
    protected override void InitEvents()
    {
        onShieldChanged = this.NewPlayerActionEvent(() =>
        {
            // Check Shield Health
            if (PlayerStats.Inst.GetShieldAt(shieldIndex).IsMin)
            {
                // Deactivate Item
                Deactivate();
            }
        });
    }

    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        // Spawn Effect
        shieldEffect = Instantiate(shieldEffectPrefab, GM.Player.transform.GetChild(0));
        shieldEffect.SetActive(false);
    }
    protected override void OnRemovedFromInventory()
    {
        base.OnRemovedFromInventory();

        // Destroy Effect
        Destroy(shieldEffect);
    }

    protected override void OnActivate()
    {
        // Stop Cooldown Timer
        CooldownTimer.SetActive(false);
        CooldownTimer.Reset();

        // Start Duration Timer
        durationTimer.SetActive(true);
        durationTimer.Reset();

        // Get Shield Index
        shieldIndex = PlayerStats.Inst.AddShield(shieldHealth);

        // Enable Shield Item Effect
        PlayerActionEventManager.AddEvent(PlayerActions.ShieldChanged, onShieldChanged);

        // Show Effect
        shieldEffect.SetActive(true);
    }
    protected override void OnDeactivate()
    {
        // Stop Duration Timer
        durationTimer.SetActive(false);
        durationTimer.Reset();

        // Remove Shield
        PlayerStats.Inst.RemoveShield(ref shieldIndex);

        // Disable Shield Item Effect
        PlayerActionEventManager.RemoveEvent(PlayerActions.ShieldChanged, onShieldChanged);

        // Hide Effect
        shieldEffect.SetActive(false);
    }
    #endregion
}
