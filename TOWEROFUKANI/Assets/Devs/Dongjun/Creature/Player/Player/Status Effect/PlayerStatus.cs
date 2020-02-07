using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : SingletonBase<PlayerStatus>
{
    private Dictionary<StatusID, Dictionary<MobAction, List<StatusEffect>>> dic_Effects = 
        new Dictionary<StatusID, Dictionary<MobAction, List<StatusEffect>>>();

    private int stunCount = 0;
    public bool IsStunned => stunCount > 0;

    public void AddStunCount() => stunCount++;
    public void RemoveStunCount() => stunCount--;

    public void Trigger(Mob_Base mob, MobAction mobAction)
    {
        if (!dic_Effects.ContainsKey(mob.StatusID))
            return;

        if (!dic_Effects[mob.StatusID].ContainsKey(mobAction))
            return;

        for (int i = dic_Effects[mob.StatusID][mobAction].Count - 1; i >= 0; i--)
        {
            if (dic_Effects[mob.StatusID][mobAction].Count == 0)
            {
                dic_Effects.Remove(mob.StatusID);
                continue;
            }

            dic_Effects[mob.StatusID][mobAction][i].OnAction?.Invoke();
        }
    }
    public void AddEffect(Mob_Base mob, MobAction mobAction, StatusEffect effect)
    {
        if (!dic_Effects.ContainsKey(mob.StatusID))
            dic_Effects.Add(mob.StatusID, new Dictionary<MobAction, List<StatusEffect>>());

        if (!dic_Effects[mob.StatusID].ContainsKey(mobAction))
            dic_Effects[mob.StatusID].Add(mobAction, new List<StatusEffect>());

        List <StatusEffect> effects = dic_Effects[mob.StatusID][mobAction];

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
        dic_Effects[key][mobAction].Remove(effect);
    }
    public void RemoveEffect(StatusType statusType)
    {
        foreach (var entry in dic_Effects)
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
        foreach (var entry in dic_Effects)
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
        foreach (var entry in dic_Effects)
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
