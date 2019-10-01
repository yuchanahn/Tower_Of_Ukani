using UnityEngine;

public abstract class CLA_Action : MonoBehaviour
{
    [HideInInspector]
    public bool CanExecuteOnStart = true;

    public virtual void OnEnter() { }
    public virtual void OnLateEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnFixedUpdate() { }
}
