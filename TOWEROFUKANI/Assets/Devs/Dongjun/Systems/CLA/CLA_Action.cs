using UnityEngine;

public abstract class CLA_Action : MonoBehaviour
{
    public bool CanExecuteOnStart = true;

    public virtual void OnChange() { }
    public virtual void OnStart() { }
    public virtual void OnEnd() { }
    public virtual void OnUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnFixedUpdate() { }
}
