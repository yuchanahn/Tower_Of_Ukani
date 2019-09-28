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

    #region Var: Inspector
    [Header("Default Parent Obj of Pooling Objects")]
    [SerializeField]
    private Transform defaultPoolParent;

    [Header("Spawn Object On Start")]
    [SerializeField]
    private StartPoolData[] startPoolData;
    #endregion

    #region Var: Pool Data
    private static Dictionary<PoolingObj, HashSet<PoolingObj>> pool_Active = new Dictionary<PoolingObj, HashSet<PoolingObj>>();
    private static Dictionary<PoolingObj, Queue<PoolingObj>> pool_Sleeping = new Dictionary<PoolingObj, Queue<PoolingObj>>();
    #endregion

    #region Method: Unity
    private void Awake()
    {
        Inst = this;

        SetUpStartPool();
    }
    #endregion

    #region Method: Private
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
                    obj.InitPoolingObj(prefab);

                    pool_Sleeping[prefab].Enqueue(obj);

                    obj.transform.SetParent(Inst.defaultPoolParent);
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
        obj.InitPoolingObj(prefab);

        pool_Active[prefab].Add(obj);

        obj.transform.SetParent(Inst.defaultPoolParent);
        obj.gameObject.SetActive(true);
        obj.ResetOnActive();

        return obj;
    }
    #endregion

    #region Method: Public
    public static GameObject Activate(PoolingObj prefab, Vector2 pos, Quaternion rot)
    {
        PoolingObj obj = ActivateObj(prefab);
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        return obj.gameObject;
    }
    public static GameObject Activate(PoolingObj prefab, Vector2 pos, Quaternion rot, Transform parent)
    {
        PoolingObj obj = ActivateObj(prefab);
        obj.transform.SetParent(parent);
        obj.transform.localPosition = pos;
        obj.transform.localRotation = rot;

        return obj.gameObject;
    }
    public static void Sleep(PoolingObj obj)
    {
        if (!pool_Active.ContainsKey(obj.Prefab) ||
            !pool_Active[obj.Prefab].Contains(obj))
            return;

        pool_Active[obj.Prefab].Remove(obj);
        pool_Sleeping[obj.Prefab].Enqueue(obj);
        obj.gameObject.SetActive(false);
    }
    #endregion
}