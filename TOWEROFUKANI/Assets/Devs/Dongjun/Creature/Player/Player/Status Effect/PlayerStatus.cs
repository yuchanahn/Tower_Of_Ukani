using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : SingletonBase<PlayerStatus>
{
    private Dictionary<StatusID, Dictionary<MobAction, List<StatusEffect>>> statusEffects = 
        new Dictionary<StatusID, Dictionary<MobAction, List<StatusEffect>>>();

    public BoolSet IsStunned
    { get; private set; } = new BoolSet();
    public bool IsKnockbacked
    { get; private set; } = false;
    public bool IsHardCCed => IsStunned.Value || IsKnockbacked;

    public void AddStun<Tthis>(Tthis _this) where Tthis : class
    {
        IsStunned.Set(_this, true);
        ActionEffectManager.Trigger(PlayerActions.Stunned);
    }
    public void RemoveStun<Tthis>(Tthis _this) where Tthis : class
    {
        IsStunned.Set(_this, false);
        if (!IsStunned.Value)
            ActionEffectManager.Trigger(PlayerActions.StunEnd);
    }

    public void Trigger(Mob_Base mob, MobAction mobAction)
    {
        if (!statusEffects.ContainsKey(mob.StatusID))
            return;

        if (!statusEffects[mob.StatusID].ContainsKey(mobAction))
            return;

        for (int i = statusEffects[mob.StatusID][mobAction].Count - 1; i >= 0; i--)
        {
            if (statusEffects[mob.StatusID][mobAction].Count == 0)
            {
                statusEffects.Remove(mob.StatusID);
                continue;
            }

            statusEffects[mob.StatusID][mobAction][i].OnAction?.Invoke();
        }
    }
    public void AddEffect(Mob_Base mob, MobAction mobAction, StatusEffect effect)
    {
        if (!statusEffects.ContainsKey(mob.StatusID))
            statusEffects.Add(mob.StatusID, new Dictionary<MobAction, List<StatusEffect>>());

        if (!statusEffects[mob.StatusID].ContainsKey(mobAction))
            statusEffects[mob.StatusID].Add(mobAction, new List<StatusEffect>());

        List <StatusEffect> effects = statusEffects[mob.StatusID][mobAction];

        if (effects.Contains(effect))
            return;

        // Invoke OnStart
        effect.OnStart?.Invoke();

        // Insert
        int insertIndex = effects.FindIndex(i => i.AfterThis == effect.GetType());
        if (insertIndex != -1)
        {
            effects.Insert(insertIndex, effect);
            return;
        }

        // Add
        effects.Add(effect);
    }

    public void RemoveFromList(StatusID key, MobAction mobAction, StatusEffect effect)
    {
        statusEffects[key][mobAction].Remove(effect);
    }
    public void RemoveEffect(StatusType statusType)
    {
        foreach (var entry in statusEffects)
        {
            foreach (var effects in entry.Value)
            {
                for (int i = 0; i < effects.Value.Count; i++)
                {
                    if (effects.Value[i].StatusType == statusType)
                    {
                        effects.Value[i].OnEnd();
                        RemoveFromList(entry.Key, effects.Key, effects.Value[i]);
                    }
                }
            }
        }
    }
    public void RemoveEffect(Mob_Base caster)
    {
        foreach (var entry in statusEffects)
        {
            foreach (var effects in entry.Value)
            {
                for (int i = 0; i < effects.Value.Count; i++)
                {
                    if (effects.Value[i].Caster == caster.gameObject)
                    {
                        effects.Value[i].OnEnd();
                        RemoveFromList(entry.Key, effects.Key, effects.Value[i]);
                    }
                }
            }
        }
    }
    public void RemoveEffect(Mob_Base caster, StatusType statusType)
    {
        foreach (var entry in statusEffects)
        {
            foreach (var effects in entry.Value)
            {
                for (int i = 0; i < effects.Value.Count; i++)
                {
                    if (effects.Value[i].Caster == caster.gameObject
                    && effects.Value[i].StatusType == statusType)
                    {
                        effects.Value[i].OnEnd();
                        RemoveFromList(entry.Key, effects.Key, effects.Value[i]);
                    }
                }
            }
        }
    }
}

public static class StatusCreator
{
    public static void CreateStatusEffect(
        this Mob_Base mob,
        MobAction mobAction,
        StatusType statusType,
        float endTime = 0,
        Action onStart = null,
        Action onEnd = null,
        Action onAction = null,
        Type afterThis = null)
    {
        PlayerStatus.Inst.AddEffect(
            mob,
            mobAction,
            new StatusEffect(
                mob.StatusID,
                mobAction,
                mob.gameObject,
                statusType,
                endTime,
                onStart,
                onEnd,
                onAction,
                afterThis));
    }

    public static void CreateStatusStun(
        this Mob_Base mob,
        MobAction mobAction,
        StatusType statusType,
        float endTime,
        Action onStart = null,
        Action onEnd = null,
        Action onAction = null,
        Type afterThis = null)
    {
        PlayerStatus.Inst.AddEffect(
            mob,
            mobAction,
            new StatusStun(
                mob.StatusID,
                mobAction,
                mob.gameObject,
                statusType,
                endTime,
                onStart,
                onEnd,
                onAction,
                afterThis));
    }
}
