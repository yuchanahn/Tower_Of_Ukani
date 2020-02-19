using System;

public struct PlayerActionEvent
{
    public Action EffectAction
    { get; private set; }
    public Type ThisType
    { get; private set; }
    public Type AfterThis
    { get; private set; }

    public PlayerActionEvent(Type thisType, Type after, Action action)
    {
        EffectAction = action;
        ThisType = thisType;
        AfterThis = after;
    }
}

public static class PlayerActionEventExtension
{
    public static PlayerActionEvent NewPlayerActionEvent<T_THis>(this T_THis _, Type afterThis, Action action)
    {
        return new PlayerActionEvent(typeof(T_THis), afterThis, action);
    }
    public static PlayerActionEvent NewPlayerActionEvent<T_THis>(this T_THis _, Action action)
    {
        return new PlayerActionEvent(typeof(T_THis), null, action);
    }
}
