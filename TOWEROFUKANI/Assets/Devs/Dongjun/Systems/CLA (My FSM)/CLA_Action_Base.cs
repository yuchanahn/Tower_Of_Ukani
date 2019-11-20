using UnityEngine;

public abstract class CLA_Action_Base : MonoBehaviour
{
    // 한번만 실행됨.
    public virtual void OnEnter() { }        // 액션을 들어온 순간 실행됨. (실행: Update)
    public virtual void OnLateEnter() { }    // 액션을 들어온 순간 실행됨. (실행: LateUpdate)
    public virtual void OnExit() { }         // 액션을 나간 순간 실행됨. (실행: Update)

    // 매프레임 실행됨.
    public virtual void OnUpdate() { }
    public virtual void OnLateUpdate() { }
    public virtual void OnFixedUpdate() { }
}
