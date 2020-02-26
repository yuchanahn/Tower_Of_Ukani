using UnityEngine;

public class SSM_State_wMain<T> : SSM_State where T : SSM_Main
{
    protected T main { get; private set; }

    protected virtual void Awake()
    {
        main = GetComponent<T>();
    }
}
