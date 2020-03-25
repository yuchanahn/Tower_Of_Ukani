using UnityEngine;

public class TalismanOfProtection : ActiveItem
{
    #region Var: Inspector
    [SerializeField] private GameObject shieldEffectPrefab;
    #endregion

    #region Var: Stats
    private TimerData durationTimer = new TimerData();

    private int shieldIndex = -1;
    private FloatStat shieldhealth;
    #endregion

    #region Var: Visual Effect
    private GameObject shieldEffect;
    #endregion

    #region Var: Item Effect
    private PlayerActionEvent onShieldChanged;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        // Player Action Event
        onShieldChanged = this.NewPlayerActionEvent(() =>
        {
            // Check Shield Health
            if (PlayerStats.Inst.GetShieldAt(shieldIndex).shieldHealth.Value == 0)
            {
                Deactivate();
            }
        });
    }
    #endregion

    #region Method: Stats
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
        shieldhealth = new FloatStat(40, min: 0, max: 40);
    }
    #endregion

    #region Method: Item
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
    #endregion

    #region Method: Activate / Deactivate
    protected override void OnActivate()
    {
        // Stop Cooldown Timer
        CooldownTimer.SetActive(false);
        CooldownTimer.Reset();

        // Start Duration Timer
        durationTimer.SetActive(true);
        durationTimer.Reset();

        // Get Shield Index
        shieldIndex = PlayerStats.Inst.AddShield(shieldhealth);

        // Enable Shield Item Effect
        PlayerActionEventManager.AddEvent(PlayerActions.ShieldChanged, onShieldChanged);

        // Show Effect
        shieldEffect.SetActive(true);
    }
    public override void Deactivate()
    {
        base.Deactivate();

        // Start Cooldown Timer
        CooldownTimer.SetActive(true);
        CooldownTimer.Reset();

        // Stop Duration Timer
        durationTimer.SetActive(false);
        durationTimer.Reset();

        // Remove Shield
        if (shieldIndex != -1)
            PlayerStats.Inst.RemoveShield(shieldIndex);
        shieldIndex = -1;

        // Disable Shield Item Effect
        PlayerActionEventManager.RemoveEvent(PlayerActions.ShieldChanged, onShieldChanged);

        // Hide Effect
        shieldEffect.SetActive(false);
    }
    #endregion
}
