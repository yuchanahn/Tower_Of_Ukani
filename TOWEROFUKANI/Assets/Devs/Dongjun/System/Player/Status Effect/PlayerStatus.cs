using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatus : SingletonBase<PlayerStatus>
{
    private static Dictionary<StatusID, List<PlayerStatusEffect>> statusEffects;

    #region Var: Hit
    // Hit
    public static BoolCount AbsorbDamage
    { get; private set; } = new BoolCount();
    public static BoolCount IgnoreDamage
    { get; private set; } = new BoolCount();
    #endregion

    #region Var: CC
    private static List<PlayerStatus_Slow> slowList;
    public static bool IsSlowed => slowList.Count != 0;

    public static BoolCount IsStunned
    { get; private set; } = new BoolCount();
    public static bool IsKnockbacked
    { get; private set; } = false;
    public static bool IsHardCCed => IsStunned.Value || IsKnockbacked;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        statusEffects = new Dictionary<StatusID, List<PlayerStatusEffect>>();

        slowList = new List<PlayerStatus_Slow>();

        AbsorbDamage = new BoolCount();
        IgnoreDamage = new BoolCount();

        IsStunned = new BoolCount();
        IsKnockbacked = false;
    }
    #endregion

    #region Method: Status Effect
    public static void AddEffect(PlayerStatusEffect effect)
    {
        if (!statusEffects.ContainsKey(effect.ID))
            statusEffects.Add(effect.ID, new List<PlayerStatusEffect>());

        List<PlayerStatusEffect> effects = statusEffects[effect.ID];

        if (effects.Contains(effect))
            return;

        // Add
        effects.Add(effect);

        // On Start
        effect.OnStart();
    }

    public static void RemoveEffect(PlayerStatusEffect effect)
    {
        if (statusEffects[effect.ID].Contains(effect))
        {
            statusEffects[effect.ID].Remove(effect);
            effect.OnEnd();
        }
    }
    public static void RemoveEffect(GameObject caster)
    {
        foreach (var entry in statusEffects)
        {
            for (int i = 0; i < entry.Value.Count; i++)
            {
                if (entry.Value[i].Caster == caster.gameObject)
                {
                    entry.Value.Remove(entry.Value[i]);
                    entry.Value[i].OnEnd();
                }
            }
        }
    }
    public static void RemoveEffect(StatusType statusType)
    {
        foreach (var entry in statusEffects)
        {
            for (int i = 0; i < entry.Value.Count; i++)
            {
                if (entry.Value[i].StatusType == statusType)
                {
                    entry.Value.Remove(entry.Value[i]);
                    entry.Value[i].OnEnd();
                }
            }
        }
    }
    public static void RemoveEffect(GameObject caster, StatusType statusType)
    {
        foreach (var entry in statusEffects)
        {
            for (int i = 0; i < entry.Value.Count; i++)
            {
                if (entry.Value[i].Caster == caster.gameObject && entry.Value[i].StatusType == statusType)
                {
                    entry.Value.Remove(entry.Value[i]);
                    entry.Value[i].OnEnd();
                }
            }
        }
    }

    public static bool HasEffect<T>() where T : PlayerStatusEffect
    {
        foreach (var entry in statusEffects)
        {
            for (int i = 0; i < entry.Value.Count; i++)
            {
                if (entry.Value[i].GetType() == typeof(T))
                    return true;
            }
        }

        return false;
    }
    public static bool HasEffect(StatusType statusType)
    {
        foreach (var entry in statusEffects)
        {
            for (int i = 0; i < entry.Value.Count; i++)
            {
                if (entry.Value[i].StatusType == statusType)
                    return true;
            }
        }

        return false;
    }
    #endregion

    #region Method: Hit
    public void AbsorbDamage_Add()
    {
        AbsorbDamage.Set(true);
    }
    public void AbsorbDamage_Remove()
    {
        AbsorbDamage.Set(false);
    }

    public void IgnoreDamage_Add()
    {
        IgnoreDamage.Set(true);
    }
    public void IgnoreDamage_Remove()
    {
        IgnoreDamage.Set(false);
    }
    #endregion

    #region Method: CC
    private void Slow()
    {
        PlayerStats.Inst.walkData.walkSpeed.ModPercent = -slowList[slowList.Count - 1].SlowAmount;
    }
    public void Slow_Add(PlayerStatus_Slow status_Slow)
    {
        slowList.Add(status_Slow);
        slowList = slowList.OrderByDescending(o => o.SlowAmount).ToList();

        // Apply Slow
        PlayerStatMod.Add_Player(Slow);
    }
    public void Slow_Remove(PlayerStatus_Slow status_Slow)
    {
        slowList.Remove(status_Slow);

        // Apply Slow
        PlayerStatMod.Remove_Player(Slow);
    }

    public void Stun_Add()
    {
        IsStunned.Set(true);
        ActionEffectManager.Trigger(PlayerActions.Stunned);
    }
    public void Stun_Remove()
    {
        IsStunned.Set(false);
        if (!IsStunned.Value) ActionEffectManager.Trigger(PlayerActions.StunEnd);
    }
    #endregion
}
