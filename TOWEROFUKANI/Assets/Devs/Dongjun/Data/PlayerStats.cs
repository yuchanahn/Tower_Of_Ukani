using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Var: Stats
    private static IntStat health = new IntStat(100, min: 0, max: 100);
    private static FloatStat stamina = new FloatStat(3f, min: 0, max: 3f);
    private static FloatStat staminaRegen = new FloatStat(0.5f, min: 0, max: 1f);
    #endregion

    #region Var: Data for Item Effect
    public static int DamageReceived;
    public static int HealReceived;
    #endregion

    #region Var: Event For UI
    private static Dictionary<GameObject, Action<IntStat>> OnHealthChange;
    private static Dictionary<GameObject, Action<FloatStat>> OnStaminaChange;
    #endregion

    #region Var: Properties
    public static IntStat Health => health;
    public static FloatStat Stamina => stamina;
    #endregion

    #region Method: Unity
    private void Awake()
    {
        OnHealthChange = new Dictionary<GameObject, Action<IntStat>>();
        OnStaminaChange = new Dictionary<GameObject, Action<FloatStat>>();
    }
    private void LateUpdate()
    {
        Regen_Stamina();
    }
    #endregion

    #region Method: Change Stat
    private static void Regen_Stamina()
    {
        if (stamina.Value == stamina.Max)
            return;

        stamina.Mod_Flat += staminaRegen.Value * Time.deltaTime;
        Invoke_OnStaminaChange();
    }
    public static bool UseStamina(int amount)
    {
        if (stamina.Value < amount)
            return false;

        stamina.Mod_Flat -= amount;
        return true;
    }

    public static void Damage(int amount)
    {
        amount = Mathf.Abs(amount);

        // Save Received Damage amount
        DamageReceived = amount;

        // Trriger Item Effect
        ItemEffectManager.Trigger(PlayerActions.Damaged);

        // Apply Damage
        health.Mod_Flat -= DamageReceived;

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
        health.Mod_Flat += HealReceived;

        Invoke_OnHealthChange();
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
            if (key is null)
            {
                OnHealthChange.Remove(key);
                continue;
            }

            OnHealthChange[key].Invoke(Health);
        }
    }

    public static void AddEvent_OnStaminaChange(GameObject slef, Action<FloatStat> action)
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
            if (key is null)
            {
                OnStaminaChange.Remove(key);
                continue;
            }

            OnStaminaChange[key].Invoke(Stamina);
        }
    }
    #endregion
}
