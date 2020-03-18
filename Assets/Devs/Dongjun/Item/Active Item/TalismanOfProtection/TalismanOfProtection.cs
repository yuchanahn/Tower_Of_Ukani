using UnityEngine;

public class TalismanOfProtection : ActiveItem
{
    #region Var: Inspector
    [SerializeField] private GameObject shieldEffectPrefab;
    #endregion

    #region Var: Stats
    private TimerData durationTimer = new TimerData();
    private FloatStat shieldhealth;
    #endregion

    #region Var: Effect
    private GameObject shieldEffect;
    #endregion

    #region Var: Item Effect
    private PlayerActionEvent onDamageReceived;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        // Player Action Event
        onDamageReceived = this.NewPlayerActionEvent(() =>
        {
            // Calculate Overkill Damage
            float overkillDmg = Mathf.Max(PlayerStats.Inst.DamageReceived - shieldhealth.Value, 0);

            // Damage Shield
            shieldhealth.ModFlat -= PlayerStats.Inst.DamageReceived;

            // Damage Player
            PlayerStats.Inst.DamageReceived = overkillDmg;

            // Deactivate Shield
            if (shieldhealth.Value == 0)
                Deactivate();
        });
    }
    #endregion

    #region Method: Stats
    public override void InitStats()
    {
        // Init Cooldown
        cooldownTimer.EndTime = 5f;

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
        cooldownTimer.SetActive(false);
        cooldownTimer.Reset();

        // Start Duration Timer
        durationTimer.SetActive(true);
        durationTimer.Reset();

        // Enable Shield Item Effect
        PlayerActionEventManager.AddEvent(PlayerActions.HealthDamaged, onDamageReceived);

        // Show Effect
        shieldEffect.SetActive(true);
    }
    public override void Deactivate()
    {
        base.Deactivate();

        // Start Cooldown Timer
        cooldownTimer.SetActive(true);
        cooldownTimer.Reset();

        // Stop Duration Timer
        durationTimer.SetActive(false);
        durationTimer.Reset();

        // Reset Shield Health
        shieldhealth.ModFlat = 0;

        // Disable Shield Item Effect
        PlayerActionEventManager.RemoveEvent(PlayerActions.HealthDamaged, onDamageReceived);

        // Hide Effect
        shieldEffect.SetActive(false);
    }
    #endregion
}
