using System;

public class ItemEffect
{
    public Type ThisType
    { get; private set; }
    public Action ItemAction
    { get; private set; }
    public Type After
    { get; private set; }

    public ItemEffect(Type thisType, Action action, Type after = null)
    {
        ThisType = thisType;
        ItemAction = action;
        After = after;
    }
}