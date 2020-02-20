using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum KnockbackMode
{
    Weak,
    Strong,
    Unstoppable
}

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
    // Slow
    private static List<PlayerStatus_Slow> slowList;
    private static Action slow = () => PlayerStats.Inst.walkData.walkSpeed.ModPercent = -slowList[0].SlowAmount;
    public static bool IsSlowed => slowList.Count != 0;

    // Knockback
    private static PlayerStatus_Knockback currentKnockback = null;
    private static KnockbackMode curKnockbackMode = KnockbackMode.Weak;
    public static Vector2 KnockbackDir
    { get; private set; }
    public static AnimationCurve KnockbackCurve
    { get; private set; }
    public static bool IsKnockbacked => KnockbackDir != Vector2.zero;

    // Stun
    public static BoolCount IsStunned
    { get; private set; } = new BoolCount();

    // Incapacitated (Hard CCed)
    public static bool Incapacitated => IsStunned.Value || IsKnockbacked;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();
        statusEffects = new Dictionary<StatusID, List<PlayerStatusEffect>>();

        AbsorbDamage = new BoolCount();
        IgnoreDamage = new BoolCount();

        slowList = new List<PlayerStatus_Slow>();

        currentKnockback = null;
        curKnockbackMode = KnockbackMode.Weak;

        IsStunned = new BoolCount();
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

        // On Start
        effect.OnStart();

        // Add
        effects.Add(effect);
    }

    public static void RemoveEffect(PlayerStatusEffect effect)
    {
        if (effect == null || !statusEffects[effect.ID].Contains(effect))
            return;

        statusEffects[effect.ID].Remove(effect);
        effect.OnEnd();
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
    public void Slow_Add(PlayerStatus_Slow status_Slow)
    {
        slowList.Add(status_Slow);
        slowList = slowList.OrderByDescending(o => o.SlowAmount).ToList();

        // Apply Slow
        PlayerStatMod.Add_Player(slow);
    }
    public void Slow_Remove(PlayerStatus_Slow status_Slow)
    {
        slowList.Remove(status_Slow);

        // Apply Slow
        PlayerStatMod.Remove_Player(slow);
    }

    public void Konckback_Add(PlayerStatus_Knockback status, KnockbackMode mode, Vector2 knockbackDir, AnimationCurve speedCurve)
    {
        if (curKnockbackMode == KnockbackMode.Unstoppable || curKnockbackMode > mode)
            return;

        RemoveEffect(currentKnockback);
        currentKnockback = status;

        curKnockbackMode = mode;
        KnockbackDir = knockbackDir;
        KnockbackCurve = speedCurve;

        PlayerActionEventManager.Trigger(PlayerActions.Knockbacked);
    }
    public void Konckback_Remove(PlayerStatus_Knockback status)
    {
        if (status != currentKnockback)
            return;

        currentKnockback = null;

        curKnockbackMode = KnockbackMode.Weak;
        KnockbackDir = Vector2.zero;
        KnockbackCurve = null;
        
        PlayerActionEventManager.Trigger(PlayerActions.KnockbackEnd);
    }

    public void Stun_Add()
    {
        IsStunned.Set(true);
        PlayerActionEventManager.Trigger(PlayerActions.Stunned);
    }
    public void Stun_Remove()
    {
        IsStunned.Set(false);
        if (!IsStunned.Value) PlayerActionEventManager.Trigger(PlayerActions.StunEnd);
    }
    #endregion
}
