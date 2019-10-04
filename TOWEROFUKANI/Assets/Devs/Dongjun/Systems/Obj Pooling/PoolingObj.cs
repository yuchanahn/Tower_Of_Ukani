using UnityEngine;

public abstract class PoolingObj : MonoBehaviour
{
    public PoolingObj Prefab { get; private set; }

    public abstract void ResetOnSpawn();

    public void InitPoolingObj(PoolingObj prefab)
    {
        Prefab = prefab;
    }
}