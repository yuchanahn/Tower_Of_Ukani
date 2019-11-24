using UnityEngine;

public abstract class ActiveItem : Item
{
    public bool IsActive
    { get; protected set; } = false;

    public virtual void Activate()
    {
        IsActive = true;
    }
    public virtual void Deactivate()
    {
        IsActive = false;
    }
}
