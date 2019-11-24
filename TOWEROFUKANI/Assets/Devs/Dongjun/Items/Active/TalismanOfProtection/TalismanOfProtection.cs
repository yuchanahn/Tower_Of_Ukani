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
        cooldownTimer.UseAutoTick(gameObject, true);
        cooldownTimer.Restart();

        onDamageReceived = new ItemEffect(GetType(), Shield);
    }
    public override void OnRemove()
    {
        base.OnRemove();

        cooldownTimer.UseAutoTick(gameObject, false);
        cooldownTimer.ToZero();

        durationTimer.UseAutoTick(gameObject, false);
        durationTimer.ToZero();

        Deactivate();
    }
    #endregion

    #region Method: Activate/Deactivate
    public override void Activate()
    {
        base.Activate();

        if (cooldownTimer.IsEnded)
        {
            cooldownTimer.SetActive(false);

            durationTimer.UseAutoTick(gameObject, true);
            durationTimer.Restart();

            ItemEffectManager.AddEffect(PlayerActions.Damaged, onDamageReceived);

            // Spawn Effect
            shieldEffect = Instantiate(shieldEffectPrefab, GM.PlayerObj.transform.GetChild(0));
        }
    }
    public override void Deactivate()
    {
        base.Deactivate();

        cooldownTimer.SetActive(true);
        cooldownTimer.Restart();

        durationTimer.UseAutoTick(gameObject, false);
        durationTimer.ToZero();

        shieldhealth.ModFlat = 0;

        ItemEffectManager.RemoveEffect(PlayerActions.Damaged, onDamageReceived);

        // Destroy Effect
        Destroy(shieldEffect);
    }
    #endregion

    #region Method: Shield
    private void Shield()
    {
        int overkillDmg = PlayerStats.DamageReceived - shieldhealth.Value;

        if (overkillDmg >= 0)
        {
            PlayerStats.DamageReceived = overkillDmg;
            Deactivate();
        }
        else
        {
            shieldhealth.ModFlat -= PlayerStats.DamageReceived;
            PlayerStats.DamageReceived = 0;
        }
    }
    #endregion
}
