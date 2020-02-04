using System;

public struct ItemEffect
{
    public Action ItemAction
    { get; private set; }
    public Type ThisType
    { get; private set; }
    public Type AfterThis
    { get; private set; }

    public ItemEffect(Action action, Type thisType, Type after = null)
    {
        ItemAction = action;
        ThisType = thisType;
        AfterThis = after;
    }
}

public static class ItemEffectExtension
{
    public static ItemEffect CreateItemEffect<T_THis, T_After>(this T_THis _, Action action)
    {
        return new ItemEffect(action, typeof(T_THis), typeof(T_After));
    }
    public static ItemEffect CreateItemEffect<T_THis>(this T_THis _, Action action)
    {
        return new ItemEffect(action, typeof(T_THis));
    }
}
