using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StartPoolData
{
    public PoolingObj prefab;
    public uint count;
}

public class ObjPoolingManager : MonoBehaviour
{
    private static ObjPoolingManager Inst;

    private static Dictionary<PoolingObj, HashSet<PoolingObj>> pool_Active = new Dictionary<PoolingObj, HashSet<PoolingObj>>();
    private static Dictionary<PoolingObj, Queue<PoolingObj>> pool_Sleeping = new Dictionary<PoolingObj, Queue<PoolingObj>>();

    [Header("Parent of Pooling Objects")]
    [SerializeField]
    private Transform poolParent;

    [Header("Spawn Object On Start")]
    [SerializeField]
    private StartPoolData[] startPoolData;

    private void Awake()
    {
        Inst = this;

        SetUpStartPool();
    }
    private void SetUpStartPool()
    {
        if (startPoolData != null)
        {
            for (int i = 0; i < startPoolData.Length; i++)
            {
                for (int count = 0; count < startPoolData[i].count; count++)
                {
                    if (startPoolData[i].prefab == null || startPoolData[i].count <= 0)
                        continue;

                    PoolingObj prefab = startPoolData[i].prefab;

                    if (!pool_Active.ContainsKey(prefab))
                    {
                        pool_Active.Add(prefab, new HashSet<PoolingObj>());
                        pool_Sleeping.Add(prefab, new Queue<PoolingObj>());
                    }

                    PoolingObj obj = Instantiate(prefab);
                    obj.prefab = prefab;
                    pool_Sleeping[prefab].Enqueue(obj);

                    obj.transform.SetParent(Inst.poolParent);
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }

    private static PoolingObj ActivateObj(PoolingObj prefab)
    {
        if (!pool_Active.ContainsKey(prefab))
        {
            pool_Active.Add(prefab, new HashSet<PoolingObj>());
            pool_Sleeping.Add(prefab, new Queue<PoolingObj>());
        }

        PoolingObj obj = pool_Sleeping[prefab].Count == 0 ? Instantiate(prefab) : pool_Sleeping[prefab].Dequeue();

        pool_Active[prefab].Add(obj);

        obj.prefab = prefab;
        obj.transform.SetParent(Inst.poolParent);
        obj.gameObject.SetActive(true);

        return obj;
    }
    public static void Activate(PoolingObj prefab, Transform transform)
    {
        PoolingObj obj = ActivateObj(prefab);

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
    }
    public static void Activate(PoolingObj prefab, Vector2 pos, Quaternion rot)
    {
        PoolingObj obj = ActivateObj(prefab);

        obj.transform.position = pos;
        obj.transform.rotation = rot;
    }
    public static void Sleep(PoolingObj obj)
    {
        if (!pool_Active.ContainsKey(obj.prefab) ||
            !pool_Active[obj.prefab].Contains(obj))
            return;

        pool_Active[obj.prefab].Remove(obj);
        pool_Sleeping[obj.prefab].Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
}