using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : SingletonBase<PlayerStats>
{
    #region Var: Base Stats
    private float base_health_Max = 100;
    private float base_stamina_Max = 3;
    private float base_staminaRegen = 0.5f;
    #endregion

    #region Var: Current Stats
    private FloatStat health;
    private FloatStat stamina;
    private FloatStat staminaRegen;

    public bool AbsorbDamage = false;
    public bool IgnoreDamage = false;
    #endregion

    #region Var: Data Stat Change Data
    [HideInInspector] public float DamageReceived;
    [HideInInspector] public float HealReceived;
    [HideInInspector] public float DamageToDeal;
    public Mob_Base KilledMob { get; private set; }
    public Mob_Base DamagedMob { get; private set; }
    #endregion

    #region Var: Event for UI
    private readonly Dictionary<GameObject, Action<FloatStat>> OnHealthChange = new Dictionary<GameObject, Action<FloatStat>>();
    private readonly Dictionary<GameObject, Action<FloatStat>> OnStaminaChange = new Dictionary<GameObject, Action<FloatStat>>();
    #endregion

    #region Var: Properties
    public bool IsDead
    { get; private set; } = false;
    public FloatStat Health => health;
    public FloatStat Stamina => stamina;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        // Init Stats
        health = new FloatStat(0, min: 0, max: base_health_Max);
        stamina = new FloatStat(0, min: 0, max: base_stamina_Max);
        staminaRegen = new FloatStat(base_staminaRegen, min: 0);

        // Init
        health.ModFlat = health.Max;
        SetStamina(stamina.Max);
    }
    private void LateUpdate()
    {
        // Regen Stamina
        RegenStamina();
    }
    #endregion

    #region Method: Change Stat (Player)
    public void ResetStats()
    {
        health.Max = base_health_Max;
        staminaRegen.Reset();
    }

    private void RegenStamina()
    {
        if (stamina.ModFlat >= stamina.Max)
        {
            stamina.ModFlat = stamina.Max;
            return;
        }

        stamina.ModFlat += staminaRegen.Value * Time.deltaTime;

        // Invoke Event
        Invoke_OnStaminaChange();
    }
    public void SetStamina(float amount)
    {
        amount = Mathf.Clamp(amount, stamina.Min, stamina.Max);
        stamina.ModFlat = amount;

        // Invoke Event
        Invoke_OnStaminaChange();
    }
    public bool UseStamina(float amount)
    {
        if (stamina.Value < amount)
            return false;

        stamina.ModFlat -= amount;

        // Invoke Event
        Invoke_OnStaminaChange();
        return true;
    }

    public void Damage(float amount)
    {
        // Check Ignore Damage
        if (IgnoreDamage)
            return;

        // Store Damage Amount
        DamageReceived = AbsorbDamage ? 0 : Mathf.Abs(amount);

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.Damaged);

        // Apply Damage
        health.ModFlat -= DamageReceived;

        // Death
        if (health.Value == 0)
        {
            Death();
            return;
        }

        // Invoke Event
        Invoke_OnHealthChange();

        // Visual Effect
        PlayerHitEft.Create(transform.position);
        GetComponent<HitColorEffect>().OnHit();
    }
    public void Heal(float amount)
    {
        // Store Heal Amount
        HealReceived = Mathf.Abs(amount);

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.Healed);

        // Apply Heal
        health.ModFlat += Mathf.Clamp(HealReceived, 0, health.Max - health.Value);

        // Invoke Event
        Invoke_OnHealthChange();

        // TODO
        // Visual Effect
    }
    public void Death()
    {
        IsDead = true;

        // TODO
        // Play Death Animation / Effect

        // Show Death Screen
    }
    #endregion

    #region Method: Change Stat (Mob / Other)
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

        // Trigger Item Effect (Hit)
        ActionEffectManager.Trigger(PlayerActions.DamageDealt);

        // Trigger Item Effect (Other)
        for (int i = 0; i < actionToTrigger.Length; i++)
            ActionEffectManager.Trigger(actionToTrigger[i]);

        // Damage Mob
        float mobHP = iDamage.Hit(attackData);

        // Trigger Item Effect (Kill)
        if (mobHP <= 0)
        {
            KilledMob = target.GetComponent<Mob_Base>();
            ActionEffectManager.Trigger(PlayerActions.Kill);
        }

        // Reset Damaged Mob
        DamagedMob = null;

        return true;
    }
    #endregion

    #region Method: Event for UI
    public void AddEvent_OnHealthChange(GameObject slef, Action<FloatStat> action)
    {
        if (OnHealthChange.ContainsKey(slef))
            return;

        OnHealthChange.Add(slef, action);
    }
    private void Invoke_OnHealthChange()
    {
        GameObject key;
        for (int i = 0; i < OnHealthChange.Count; i++)
        {
            key = OnHealthChange.ElementAt(i).Key;
            if (key == null)
            {
                OnHealthChange.Remove(key);
                continue;
            }

            OnHealthChange[key].Invoke(health);
        }
    }

    public void AddEvent_OnStaminaChange(GameObject slef, Action<FloatStat> action)
    {
        if (OnStaminaChange.ContainsKey(slef))
            return;

        OnStaminaChange.Add(slef, action);
    }
    private void Invoke_OnStaminaChange()
    {
        GameObject key;
        for (int i = 0; i < OnStaminaChange.Count; i++)
        {
            key = OnStaminaChange.ElementAt(i).Key;
            if (key == null)
            {
                OnStaminaChange.Remove(key);
                continue;
            }

            OnStaminaChange[key].Invoke(stamina);
        }
    }
    #endregion
}