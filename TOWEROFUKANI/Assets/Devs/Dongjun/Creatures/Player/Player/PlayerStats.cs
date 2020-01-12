using System;
using System.Collections.Generic;
using System.Linq;
using Dongjun.Helper;
using UnityEngine;

public class PlayerStats : SingletonBase<PlayerStats>
{
    #region Var: Stats
    private IntStat health = new IntStat(100, min: 0, max: 100);
    private FloatStat stamina = new FloatStat(0, min: 0, max: 3);
    private FloatStat staminaRegen = new FloatStat(0.5f, min: 0);

    public bool AbsorbDamage = false;
    public bool IgnoreDamage = false;
    #endregion

    #region Var: Data for Item Effect
    public int DamageReceived;
    public int HealReceived;
    public int DamageToDeal;
    #endregion

    #region Var: Event On Stat Change
    private Dictionary<GameObject, Action<IntStat>> OnHealthChange = new Dictionary<GameObject, Action<IntStat>>();
    private Dictionary<GameObject, Action<FloatStat>> OnStaminaChange = new Dictionary<GameObject, Action<FloatStat>>();
    #endregion

    #region Var: Properties
    public bool IsDead
    { get; private set; } = false;

    public IntStat Health => health;
    public FloatStat Stamina => stamina;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        SetStamina(stamina.Max);
    }
    private void LateUpdate()
    {
        RegenStamina();
    }
    #endregion

    #region Method: Change Stat (Self)
    private void RegenStamina()
    {
        if (stamina.ModFlat >= stamina.Max)
        {
            stamina.ModFlat = stamina.Max;
            return;
        }

        stamina.ModFlat += staminaRegen.Value * Time.deltaTime;
        Invoke_OnStaminaChange();
    }
    public void SetStamina(float amount)
    {
        amount = Mathf.Clamp(amount, stamina.Min, stamina.Max);
        stamina.ModFlat = amount;
    }
    public bool UseStamina(float amount)
    {
        if (stamina.Value < amount)
            return false;

        stamina.ModFlat -= amount;
        return true;
    }

    public void Damage(int amount)
    {
        if (IgnoreDamage)
            return;

        // Apply Damage
        DamageReceived = AbsorbDamage ? 0 : Mathf.Abs(amount);
        ItemEffectManager.Trigger(PlayerActions.Damaged);
        health.ModFlat -= DamageReceived;

        // Death
        if (health.Value == 0)
        {
            //Death();
            return;
        }

        // Invoke Event
        Invoke_OnHealthChange();

        // Visual Effect
        PlayerHitEft.Create(GM.PlayerPos);
        GM.Player.GetComponent<HitColorEffect>().OnHit();
    }
    public void Heal(int amount)
    {
        // Apply Heal
        HealReceived = Mathf.Abs(amount);
        ItemEffectManager.Trigger(PlayerActions.Healed);
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
        // Play Death Animation

        // Show Death Screen
    }
    #endregion

    #region Method: Change Stat (Other)
    public bool DealDamage(IDamage iDamage, AttackData attackData)
    {
        if (iDamage == null)
            return false;

        // Set Stat
        DamageToDeal = attackData.damage.Value;

        // Trigger Item Effect
        ItemEffectManager.Trigger(PlayerActions.Hit);

        // Damage Mob
        iDamage.Hit(DamageToDeal); // 여기서 체력이나 불리언 반환 해줘!!

        // TO DO:
        // Kill Item Effect Trigger

        return true;
    }
    public bool DealDamage(IDamage iDamage, AttackData attackData, params PlayerActions[] actionToTrigger)
    {
        if (iDamage == null)
            return false;

        // Set Stat
        DamageToDeal = attackData.damage.Value;

        // Trigger Item Effect
        ItemEffectManager.Trigger(PlayerActions.Hit);

        for (int i = 0; i < actionToTrigger.Length; i++)
            ItemEffectManager.Trigger(actionToTrigger[i]);

        // Damage Mob
        iDamage.Hit(DamageToDeal); // 여기서 체력이나 불리언 반환 해줘!!

        // TO DO:
        // Kill Item Effect Trigger

        return true;
    }
    #endregion

    #region Method: On Stat Change Event
    public void AddEvent_OnHealthChange(GameObject slef, Action<IntStat> action)
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
