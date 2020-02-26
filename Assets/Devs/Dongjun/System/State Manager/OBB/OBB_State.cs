using UnityEngine;

public interface IOBB_State
{
    void InitData(OBB_Data data);
    void Init();

    void OnEnter();
    void OnLateEnter();
    void OnExit();
    void OnUpdate();
    void OnLateUpdate();
    void OnFixedUpdate();
}
public abstract class OBB_State : MonoBehaviour, IOBB_State
{
    public abstract void InitData(OBB_Data data);
    public virtual void Init() { }

    public virtual void OnEnter() { }
    public virtual void OnLateEnter() { }
    public virtual void OnExit() { }
    public virtual void OnUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnFixedUpdate() { }
}