﻿using UnityEngine;

public class TalismanOfProtection : ActiveItem
{
    #region Var: Inspector
    [SerializeField] private GameObject shieldEffectPrefab;
    #endregion

    #region Var: Stats
    private FloatStat shieldhealth;
    private TimerData durationTimer = new TimerData();
    #endregion

    #region Var: Item Effect
    private PlayerActionEvent onDamageReceived;
    #endregion

    #region Var: Effect
    private GameObject shieldEffect;
    #endregion

    #region Method: Stats
    public override void InitStats()
    {
        // Init Cooldown
        cooldownTimer.EndTime = 5f;

        // Init Duration
        durationTimer.EndTime = 2.5f;
        durationTimer.SetTick(gameObject).SetAction(onEnd: Deactivate);

        // Init Shield HP
        shieldhealth = new FloatStat(40, min: 0, max: 40);
    }
    #endregion

    #region Method: Add / Drop
    public override void OnAdd(InventoryBase inventory)
    {
        base.OnAdd(inventory);

        onDamageReceived = this.NewPlayerActionEvent(Shield);

        // Spawn Effect
        shieldEffect = Instantiate(shieldEffectPrefab, GM.PlayerObj.transform.GetChild(0));
        shieldEffect.SetActive(false);
    }
    protected override void OnRemovedFromInventory()
    {
        // Stop Timers
        durationTimer.SetActive(false);
        durationTimer.Reset();

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
        PlayerActionEventManager.AddEvent(PlayerActions.Damaged, onDamageReceived);

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
        PlayerActionEventManager.RemoveEvent(PlayerActions.Damaged, onDamageReceived);

        // Hide Effect
        shieldEffect.SetActive(false);
    }
    #endregion

    #region Method: Shield
    private void Shield()
    {
        // Calculate Overkill Damage
        float overkillDmg = Mathf.Max(PlayerStats.Inst.DamageReceived - shieldhealth.Value, 0);

        // Damage Player
        PlayerStats.Inst.DamageReceived = overkillDmg;

        // Damage Shield
        shieldhealth.ModFlat -= PlayerStats.Inst.DamageReceived;

        // Deactivate Shield
        if (shieldhealth.Value == 0)
            Deactivate();
    }
    #endregion
}
