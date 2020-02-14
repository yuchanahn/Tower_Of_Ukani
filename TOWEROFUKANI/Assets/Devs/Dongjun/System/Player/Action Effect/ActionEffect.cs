using System;

public struct ActionEffect
{
    public Action EffectAction
    { get; private set; }
    public Type ThisType
    { get; private set; }
    public Type AfterThis
    { get; private set; }

    public ActionEffect(Action action, Type thisType, Type after = null)
    {
        EffectAction = action;
        ThisType = thisType;
        AfterThis = after;
    }
}

public static class ActionEffectExtension
{
    public static ActionEffect CreateItemEffect<T_THis, T_After>(this T_THis _, Action action)
    {
        return new ActionEffect(action, typeof(T_THis), typeof(T_After));
    }
    public static ActionEffect CreateActionEffect<T_THis>(this T_THis _, Action action)
    {
        return new ActionEffect(action, typeof(T_THis));
    }
}
