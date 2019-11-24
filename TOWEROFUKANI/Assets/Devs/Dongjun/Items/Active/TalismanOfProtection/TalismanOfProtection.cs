using UnityEngine;

public class TalismanOfProtection : ActiveItem
{
    #region Var: Inspector
    [SerializeField] private GameObject shieldEffectPrefab;
    #endregion

    #region Var: Stats
    private TimerData cooldownTimer = new TimerData();
    private TimerData durationTimer = new TimerData();
    private IntStat shieldhealth;
    #endregion

    #region Var: Item Effect
    private ItemEffect onDamageReceived;
    #endregion

    #region Var: Effect
    private GameObject shieldEffect;
    #endregion

    #region Method: Unity
    private void Start()
    {
        cooldownTimer.EndTime = 0.1f;

        durationTimer.EndTime = 2.5f;
        durationTimer.SetAction(OnEnd: Deactivate);

        shieldhealth = new IntStat(40, min: 0, max: 40);
    }
    #endregion

    #region Method: Add/Remove
    public override void OnAdd()
    {
        // Set Up Timers
        cooldownTimer.SetTick(gameObject);
        cooldownTimer.Restart();

        // Initialize Item Effect
        onDamageReceived = new ItemEffect(GetType(), ShieldFunction);

        // Spawn Effect
        shieldEffect = Instantiate(shieldEffectPrefab, GM.PlayerObj.transform.GetChild(0));
        shieldEffect.SetActive(false);
    }
    public override void OnRemove()
    {
        base.OnRemove();

        // Remove Timers
        cooldownTimer.SetTick(gameObject, TimerTick.None);
        cooldownTimer.ToZero();
        durationTimer.SetTick(gameObject, TimerTick.None);
        durationTimer.ToZero();

        // Destroy Effect
        Destroy(shieldEffect);

        // Deactivate This Item
        Deactivate();
    }
    #endregion

    #region Method: Activate/Deactivate
    public override void Activate()
    {
        base.Activate();

        if (!cooldownTimer.IsEnded)
            return;

        // Stop Cooldown
        cooldownTimer.SetActive(false);

        // Start Duration
        durationTimer.SetTick(gameObject);
        durationTimer.Restart();

        // Enable Shield Item Effect
        ItemEffectManager.AddEffect(PlayerActions.Damaged, onDamageReceived);

        // Show Effect
        shieldEffect.SetActive(true);
    }
    public override void Deactivate()
    {
        base.Deactivate();

        // Start Cooldown
        cooldownTimer.SetActive(true);
        cooldownTimer.Restart();

        // Stop Duration
        durationTimer.SetTick(gameObject, TimerTick.None);
        durationTimer.ToZero();

        // Reset Shield Health
        shieldhealth.ModFlat = 0;

        // Disable Shield Item Effect
        ItemEffectManager.RemoveEffect(PlayerActions.Damaged, onDamageReceived);

        // Hide Effect
        shieldEffect.SetActive(false);
    }
    #endregion

    #region Method: Shield
    private void ShieldFunction()
    {
        int overkillDmg = PlayerStats.DamageReceived - shieldhealth.Value;

        // Shield Destroyed
        if (overkillDmg >= 0)
        {
            PlayerStats.DamageReceived = overkillDmg;
            Deactivate();
        }
        // Damage Shield
        else
        {
            shieldhealth.ModFlat -= PlayerStats.DamageReceived;
            PlayerStats.DamageReceived = 0;
        }
    }
    #endregion
}
