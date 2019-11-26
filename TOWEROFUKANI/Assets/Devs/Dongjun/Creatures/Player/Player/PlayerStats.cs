using System;
using System.Collections.Generic;
using System.Linq;
using Dongjun.Helper;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Var: Stats
    private static IntStat health = new IntStat(100, min: 0, max: 100);

    private static IntStat stamina = new IntStat(0, min: 0, max: 3);
    private static FloatStat staminaRegen = new FloatStat(0.5f, min: 0, max: 1f);
    private static float staminaUIBarValue;
    #endregion

    #region Var: Data for Item Effect
    public static int DamageReceived;
    public static int HealReceived;

    public static int DamageDealt;
    #endregion

    #region Var: Event For UI
    private static Dictionary<GameObject, Action<IntStat>> OnHealthChange;
    private static Dictionary<GameObject, Action<float>> OnStaminaChange;
    #endregion

    #region Var: Properties
    public static bool AbsorbDamage { get; set; } = false;
    public static IntStat Health => health;
    public static IntStat Stamina => stamina;
    #endregion

    #region Method: Unity
    private void Awake()
    {
        // Reset Events
        OnHealthChange = new Dictionary<GameObject, Action<IntStat>>();
        OnStaminaChange = new Dictionary<GameObject, Action<float>>();

        SetStamina(stamina.Max);
    }
    private void LateUpdate()
    {
        Regen_Stamina();
    }
    #endregion

    #region Method: Change Stat
    private static void Regen_Stamina()
    {
        if (staminaUIBarValue >= stamina.Max)
        {
            staminaUIBarValue = stamina.Max;
            stamina.ModFlat = stamina.Max;
            return;
        }

        staminaUIBarValue += staminaRegen.Value * Time.deltaTime;
        stamina.ModFlat = (int)staminaUIBarValue;
        Invoke_OnStaminaChange();
    }
    public static void SetStamina(int amount)
    {
        amount = Mathf.Clamp(amount, stamina.Min, stamina.Max);
        stamina.ModFlat = amount;
        staminaUIBarValue = amount;
    }
    public static bool UseStamina(int amount)
    {
        if (stamina.Value < amount)
            return false;

        staminaUIBarValue -= amount;
        stamina.ModFlat -= amount;
        return true;
    }

    public static void Damage(int amount)
    {
        // Save Received Damage amount
        DamageReceived = AbsorbDamage ? 0 : Mathf.Abs(amount);

        // Trriger Item Effect
        ItemEffectManager.Trigger(PlayerActions.Damaged);

        health.ModFlat -= DamageReceived;

        if (health.Value == 0)
        {
            //Death();
        }

        Invoke_OnHealthChange();

        // Visual Effect
        PlayerHitEft.Create(GM.PlayerPos);
    }
    public static void Heal(int amount)
    {
        amount = Mathf.Abs(amount);

        // Save Received Heal amount
        HealReceived = amount;

        // Trriger Item Effect
        ItemEffectManager.Trigger(PlayerActions.Healed);

        // Apply Heal
        HealReceived = Mathf.Clamp(HealReceived, 0, health.Max - health.Value);
        health.ModFlat += HealReceived;

        Invoke_OnHealthChange();
    }
    public static void Death()
    {
        health.Reset();
    }
    #endregion

    #region Method: On Stat Change Event
    public static void AddEvent_OnHealthChange(GameObject slef, Action<IntStat> action)
    {
        if (OnHealthChange.ContainsKey(slef))
            return;

        OnHealthChange.Add(slef, action);
    }
    private static void Invoke_OnHealthChange()
    {
        GameObject key;
        for (int i = 0; i < OnHealthChange.Count; i++)
        {
            key = OnHealthChange.ElementAt(i).Key;
            if (key.IsNull())
            {
                OnHealthChange.Remove(key);
                continue;
            }

            OnHealthChange[key].Invoke(Health);
        }
    }

    public static void AddEvent_OnStaminaChange(GameObject slef, Action<float> action)
    {
        if (OnStaminaChange.ContainsKey(slef))
            return;

        OnStaminaChange.Add(slef, action);
    }
    private static void Invoke_OnStaminaChange()
    {
        GameObject key;
        for (int i = 0; i < OnStaminaChange.Count; i++)
        {
            key = OnStaminaChange.ElementAt(i).Key;
            if (key.IsNull())
            {
                OnStaminaChange.Remove(key);
                continue;
            }

            OnStaminaChange[key].Invoke(staminaUIBarValue);
        }
    }
    #endregion
}
