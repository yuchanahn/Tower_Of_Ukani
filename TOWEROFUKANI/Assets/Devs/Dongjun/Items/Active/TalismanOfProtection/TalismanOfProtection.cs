using UnityEngine;

public class TalismanOfProtection : ActiveItem
{
    #region Var: Inspector
    [SerializeField] private GameObject shieldEffectPrefab;
    #endregion

    #region Var: Stats
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
        cooldownTimer.EndTime = 5f;

        durationTimer.EndTime = 2.5f;
        durationTimer.SetAction(OnEnd: Deactivate);

        shieldhealth = new IntStat(40, min: 0, max: 40);
    }
    #endregion

    #region Method: Add/Remove
    public override void OnAdd()
    {
        base.OnAdd();

        // Initialize Item Effect
        onDamageReceived = new ItemEffect(GetType(), Shield);

        // Spawn Effect
        shieldEffect = Instantiate(shieldEffectPrefab, GM.PlayerObj.transform.GetChild(0));
        shieldEffect.SetActive(false);
    }
    public override void OnRemove()
    {
        base.OnRemove();

        // Remove Timers
        durationTimer.SetTick(gameObject, TickType.None);
        durationTimer.ToZero();

        // Destroy Effect
        Destroy(shieldEffect);
    }
    #endregion

    #region Method: Activate/Deactivate
    public override void Activate()
    {
        IsActive = true;

        // Stop Cooldown Timer
        cooldownTimer.SetActive(false);
        cooldownTimer.ToZero();

        // Start Duration Timer
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

        // Start Cooldown Timer
        cooldownTimer.SetActive(true);
        cooldownTimer.Restart();

        // Stop Duration Timer
        durationTimer.SetTick(gameObject, TickType.None);
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
    private void Shield()
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
