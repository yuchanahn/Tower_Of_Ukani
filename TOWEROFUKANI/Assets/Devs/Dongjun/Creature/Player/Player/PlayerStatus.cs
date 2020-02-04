using System;
using System.Collections.Generic;
using UnityEngine;

public struct StatusEffect
{
    public Action OnStart
    { get; private set; }
    public TimerData timer
    { get; private set; }
    public Type ThisType
    { get; private set; }
    public Type AfterType
    { get; private set; }

    public StatusEffect(Action onStart, TimerData timerData, Type thisType, Type afterType = null)
    {
        OnStart = onStart;
        timer = timerData;
        ThisType = thisType;
        AfterType = afterType;
    }
}

public class PlayerStatus : SingletonBase<PlayerStatus>
{
    public enum Effect
    {
        
    }

    private Dictionary<Effect, List<StatusEffect>> effects = new Dictionary<Effect, List<StatusEffect>>();

    public void StartEffect(Effect effect, Action action)
    {

    }
    public void EndEffect()
    {

    }
}
