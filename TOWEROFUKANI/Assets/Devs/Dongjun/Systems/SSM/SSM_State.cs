using UnityEngine;

public abstract class SSM_State : MonoBehaviour
{
    public virtual void OnEnter() { }
    public virtual void OnLateEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnFixedUpdate() { }
}
