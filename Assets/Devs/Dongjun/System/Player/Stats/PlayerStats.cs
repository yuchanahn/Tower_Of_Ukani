using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : SingletonBase<PlayerStats>
{
    #region Var: Inspector
    [SerializeField] private SelfSleepObj healEffectPrefab;
    #endregion

    #region Var: Player Stats
    [HideInInspector] public FloatStat health;

    private int shieldCount = 0;
    public class ShieldHealth { public FloatStat shieldHealth; }
    [HideInInspector] public SortedDictionary<int, ShieldHealth> shields = new SortedDictionary<int, ShieldHealth>();

    [HideInInspector] public FloatStat mana;
    [HideInInspector] public FloatStat manaRegen;

    [HideInInspector] public FloatStat stamina;
    [HideInInspector] public FloatStat staminaRegen;

    [HideInInspector] public PlayerWalkData walkData;
    #endregion

    #region Var: Stat Data
    [HideInInspector] public float DamageToReceive;
    [HideInInspector] public float HealToReceive;

    [HideInInspector] public float DamageToDeal;
    public Mob_Base DamagedMob
    { get; private set; }
    public Mob_Base KilledMob
    { get; private set; }
    #endregion

    #region Prop: 
    public bool IsDead => health.Value == 0;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        InitStats();
    }
    private void LateUpdate()
    {
        RegenMana();
        RegenStamina();
    }
    #endregion

    #region Method: General
    private void InitStats()
    {
        // Health
        health = new FloatStat(100, min: 0, max: 100);

        // Mana
        mana = new FloatStat(50, min: 0, max: 50);
        //manaRegen = new FloatStat(0.2f, min: 0);
        manaRegen = new FloatStat(5f, min: 0);

        // Stamina
        stamina = new FloatStat(3, min: 0, max: 3);
        staminaRegen = new FloatStat(0.5f, min: 0);

        // Walk
        walkData = new PlayerWalkData(new FloatStat(7, min: 0), 0.2f, 0.2f);
    }
    public void ResetStats()
    {
        // Reset Health
        health.ResetMinMax();

        mana.ResetMinMax();
        manaRegen.Reset();

        stamina.ResetMinMax();
        staminaRegen.Reset();

        walkData.walkSpeed.Reset();
    }
    #endregion

    #region Method: Health
    public void Damage(AttackData attackData)
    {
        // Check Ignore Damage
        if (PlayerStatus.IgnoreDamage.Value)
            return;

        // Store Damage Amount
        DamageToReceive = PlayerStatus.AbsorbDamage.Value ? 0 : attackData.damage.Value;

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.Damaged);

        if (DamageToReceive == 0)
            return;

        DamageToReceive = DamageShield(DamageToReceive);

        if (DamageToReceive == 0)
            return;

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.HealthDamaged);

        // Apply Damage
        health.ModFlat -= DamageToReceive;

        // Death
        if (health.Value == 0)
            Death();

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.HealthChanged);

        if (IsDead)
            return;

        // Visual Effect
        PlayerHitEft.Create(transform.position);
        GetComponent<HitColorEffect>().OnHit();
    }
    public void Heal(float amount)
    {
        if (amount <= 0)
            return;

        // Store Heal Amount
        HealToReceive = amount;

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.HealthHealed);

        // Apply Heal
        health.ModFlat += Mathf.Clamp(HealToReceive, 0, health.Max - health.Value);

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.HealthChanged);

        // Visual Effect
        healEffectPrefab.Spawn(transform, Vector2.zero);
    }
    public void Death()
    {
        // TODO
        // Play Death Animation / Effect
        // Show Death Screen
    }
    #endregion

    #region Method: Shield
    public int AddShield(FloatStat shieldHealth)
    {
        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.ShieldGained);

        shields.Add(shieldCount, new ShieldHealth() { shieldHealth = shieldHealth });

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.ShieldChanged);

        return shieldCount++;
    }
    public void RemoveShield(ref int shieldIndex)
    {
        if (shieldIndex < 0)
            return;

        if (shields[shieldIndex].shieldHealth.Value > 0)
        {
            // Trigger Event
            PlayerActionEventManager.Trigger(PlayerActions.ShieldChanged);
        }

        shields.Remove(shieldIndex);

        shieldIndex = -1;

        if (shields.Count == 0)
            shieldCount = 0;
    }
    public ref FloatStat GetShieldAt(int shieldIndex)
    {
        return ref shields[shieldIndex].shieldHealth;
    }

    public float GetTotalShieldHealth()
    {
        if (shields.Count == 0)
            return 0;

        float total = 0;
        foreach (var shield in shields)
        {
            ShieldHealth box = shield.Value;

            if (box.shieldHealth.Value == 0)
                continue;

            total += box.shieldHealth.Value;
        }

        return total;
    }
    private float DamageShield(float damageAmount)
    {
        foreach (var shield in shields.Reverse())
        {
            ShieldHealth box = shield.Value;

            if (box.shieldHealth.Value == 0)
                continue;

            // Calculate Overkill Damage
            float overkillDmg = Mathf.Max(damageAmount - box.shieldHealth.Value, 0);

            // Trigger Event
            PlayerActionEventManager.Trigger(PlayerActions.ShieldDamaged);

            // Damage Shield
            box.shieldHealth.ModFlat -= damageAmount;

            // Trigger Event
            PlayerActionEventManager.Trigger(PlayerActions.ShieldChanged);

            // Damage Player
            damageAmount = overkillDmg;

            if (damageAmount == 0)
                return 0;
        }

        return damageAmount;
    }
    public void KillAllShields()
    {
        float shieldTotal = GetTotalShieldHealth();
        if (shieldTotal == 0)
            return;

        DamageShield(shieldTotal);
    }
    #endregion

    #region Method: Mana
    public void GainMana(float amount)
    {
        if (amount <= 0)
            return;

        PlayerActionEventManager.Trigger(PlayerActions.ManaGained);
        mana.ModFlat += amount;
        PlayerActionEventManager.Trigger(PlayerActions.ManaChanged);
    }
    public bool UseMana(float amount)
    {
        if (mana.Value < amount)
            return false;

        PlayerActionEventManager.Trigger(PlayerActions.ManaUsed);
        mana.ModFlat -= amount;
        PlayerActionEventManager.Trigger(PlayerActions.ManaChanged);
        return true;
    }
    public void RegenMana()
    {
        if (mana.Value >= mana.Max)
        {
            mana.ModFlat = 0;
            return;
        }

        GainMana(manaRegen.Value * Time.deltaTime);
    }
    #endregion

    #region Method: Stamina
    public void GainStamina(float amount)
    {
        if (amount <= 0)
            return;

        PlayerActionEventManager.Trigger(PlayerActions.StaminaGained);
        stamina.ModFlat += amount;
        PlayerActionEventManager.Trigger(PlayerActions.StaminaChanged);
    }
    public bool UseStamina(float amount)
    {
        if (stamina.Value < amount)
            return false;

        PlayerActionEventManager.Trigger(PlayerActions.StaminaUsed);
        stamina.ModFlat -= amount;
        PlayerActionEventManager.Trigger(PlayerActions.StaminaChanged);
        return true;
    }
    private void RegenStamina()
    {
        if (stamina.Value >= stamina.Max)
        {
            stamina.ModFlat = 0;
            return;
        }

        GainStamina(staminaRegen.Value * Time.deltaTime);
    }
    #endregion

    #region Method: Deal Damage
    public bool DealDamage(AttackData attackData, GameObject target, params PlayerActions[] actionToTrigger)
    {
        // Get IDamage
        IDamage iDamage = target.GetComponent<IDamage>();
        if (iDamage == null)
            return false;

        // Check Ignore Hit (피해 무시)
        if (target.GetComponent<StatusEffect_IgnoreHit>() != null)
            return false;

        // Store Damage
        DamageToDeal = attackData.damage.Value;

        // Store Damaged Mob
        DamagedMob = target.GetComponent<Mob_Base>();

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.DamageDealt);
        for (int i = 0; i < actionToTrigger.Length; i++)
            PlayerActionEventManager.Trigger(actionToTrigger[i]);

        attackData.damage = new FloatStat(DamageToDeal);

        if (iDamage.CheckDeath(attackData))
        {
            KilledMob = target.GetComponent<Mob_Base>();
            PlayerActionEventManager.Trigger(PlayerActions.Kill);
        }

        // Damage Mob
        iDamage.Hit(attackData);

        // Reset Damaged Mob
        DamagedMob = null;

        return true;
    }
    #endregion
}