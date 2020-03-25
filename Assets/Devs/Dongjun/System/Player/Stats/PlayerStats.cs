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
    public class ShieldHealth
    {
        public FloatStat shieldHealth;
    }
    [HideInInspector] public SortedDictionary<int, ShieldHealth> shields = new SortedDictionary<int, ShieldHealth>();

    [HideInInspector] public FloatStat mana;
    [HideInInspector] public FloatStat manaRegen;

    [HideInInspector] public FloatStat stamina;
    [HideInInspector] public FloatStat staminaRegen;

    [HideInInspector] public PlayerWalkData walkData;
    #endregion

    #region Var: Stat Change Data
    [HideInInspector] public float DamageReceived;
    [HideInInspector] public float HealReceived;
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

        // Init Health
        health = new FloatStat(100, min: 0, max: 100);

        // Init Mana
        mana = new FloatStat(50, min: 0, max: 50);
        //manaRegen = new FloatStat(0.2f, min: 0);
        manaRegen = new FloatStat(5f, min: 0);

        // Init Stamina
        stamina = new FloatStat(3, min: 0, max: 3);
        staminaRegen = new FloatStat(0.5f, min: 0);

        // Init WalkData
        walkData = new PlayerWalkData(new FloatStat(7, min: 0), 0.2f, 0.2f);
    }
    private void LateUpdate()
    {
        RegenMana();
        RegenStamina();
    }
    #endregion

    #region Method: Reset Stats
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
    public void Damage(float amount)
    {
        // Check Ignore Damage
        if (PlayerStatus.IgnoreDamage.Value)
            return;

        // Store Damage Amount
        DamageReceived = PlayerStatus.AbsorbDamage.Value ? 0 : amount;

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.Damaged);

        if (DamageReceived == 0)
            return;

        foreach (var shield in shields.Reverse())
        {
            if (shield.Value.shieldHealth.Value == 0)
                continue;

            // Calculate Overkill Damage
            float overkillDmg = Mathf.Max(DamageReceived - shield.Value.shieldHealth.Value, 0);

            // Trigger Item Effect
            PlayerActionEventManager.Trigger(PlayerActions.ShieldDamaged);

            // Damage Shield
            shield.Value.shieldHealth.ModFlat -= DamageReceived;

            // Trigger Item Effect
            PlayerActionEventManager.Trigger(PlayerActions.ShieldChanged);

            // Damage Player
            DamageReceived = overkillDmg;

            if (DamageReceived == 0)
                return;
        }

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.HealthDamaged);

        // Apply Damage
        health.ModFlat -= DamageReceived;

        // Death
        if (health.Value == 0)
        {
            Death();
            return;
        }

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.HealthChanged);

        // Visual Effect
        PlayerHitEft.Create(transform.position);
        GetComponent<HitColorEffect>().OnHit();
    }
    public void Damage(AttackData attackData)
    {
        // Check Ignore Damage
        if (PlayerStatus.IgnoreDamage.Value)
            return;

        // Store Damage Amount
        DamageReceived = PlayerStatus.AbsorbDamage.Value ? 0 : attackData.damage.Value;

        if (DamageReceived == 0)
            return;

        foreach (var shield in shields.Reverse())
        {
            if (shield.Value.shieldHealth.Value == 0)
                continue;

            // Calculate Overkill Damage
            float overkillDmg = Mathf.Max(DamageReceived - shield.Value.shieldHealth.Value, 0);

            // Trigger Item Effect
            PlayerActionEventManager.Trigger(PlayerActions.ShieldDamaged);

            // Damage Shield
            shield.Value.shieldHealth.ModFlat -= DamageReceived;

            // Trigger Item Effect
            PlayerActionEventManager.Trigger(PlayerActions.ShieldChanged);

            // Damage Player
            DamageReceived = overkillDmg;

            if (DamageReceived == 0)
                return;
        }

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.HealthDamaged);

        // Apply Damage
        health.ModFlat -= DamageReceived;

        // Death
        if (health.Value == 0)
        {
            Death();
            return;
        }

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.HealthChanged);

        // Visual Effect
        PlayerHitEft.Create(transform.position);
        GetComponent<HitColorEffect>().OnHit();
    }
    public void Heal(float amount)
    {
        if (amount <= 0)
            return;

        // Store Heal Amount
        HealReceived = amount;

        // Trigger Event
        PlayerActionEventManager.Trigger(PlayerActions.HealthHealed);

        // Apply Heal
        health.ModFlat += Mathf.Clamp(HealReceived, 0, health.Max - health.Value);

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
        shields.Add(shieldCount, new ShieldHealth() { shieldHealth = shieldHealth });
        return shieldCount++;
    }
    public void RemoveShield(int shieldIndex)
    {
        shields.Remove(shieldIndex);

        if (shields.Count == 0)
            shieldCount = 0;
    }
    public ShieldHealth GetShieldAt(int shieldIndex)
    {
        return shields[shieldIndex];
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