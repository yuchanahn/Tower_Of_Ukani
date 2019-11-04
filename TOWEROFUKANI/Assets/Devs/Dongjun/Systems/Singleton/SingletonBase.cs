using UnityEngine;

public abstract class SingletonBase<T> : MonoBehaviour
    where T : MonoBehaviour
{
    public static T Inst { get; private set; }

    protected virtual void OnEnable()
    {
        Inst = this as T;
    }
}
