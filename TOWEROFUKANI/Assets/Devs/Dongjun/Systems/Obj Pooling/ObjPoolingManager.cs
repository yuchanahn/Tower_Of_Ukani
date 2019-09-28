using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct StartPoolData
{
    public PoolingObj prefab;
    public uint count;
    public bool unlimited;
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
        if (startPoolData == null)
            return;

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
    private static PoolingObj ActivateObj(PoolingObj prefab, bool canCreateNew)
    {
        PoolingObj obj;

        if (!pool_Active.ContainsKey(prefab))
        {
            pool_Active.Add(prefab, new HashSet<PoolingObj>());
            pool_Sleeping.Add(prefab, new Queue<PoolingObj>());

            obj = Instantiate(prefab);
            obj.InitPoolingObj(prefab);

            pool_Active[prefab].Add(obj);
            obj.gameObject.SetActive(true);
            obj.ResetOnActive();
            return obj;
        }

        obj = pool_Sleeping[prefab].Count == 0 ? (canCreateNew ? Instantiate(prefab) : pool_Active[prefab].FirstOrDefault()) : pool_Sleeping[prefab].Dequeue();

        obj.InitPoolingObj(prefab);

        pool_Active[prefab].Add(obj);
        obj.gameObject.SetActive(true);
        obj.ResetOnActive();
        return obj;
    }
    #endregion

    #region Method: Public
    public static void SetUpObjPool(StartPoolData startPoolData)
    {
        for (int count = 0; count < startPoolData.count; count++)
        {
            if (startPoolData.prefab == null || startPoolData.count <= 0)
                continue;

            PoolingObj prefab = startPoolData.prefab;

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
    public static GameObject Activate(PoolingObj prefab, Vector2 pos, Quaternion rot, bool canCreateNew = true)
    {
        PoolingObj obj = ActivateObj(prefab, canCreateNew);
        obj.transform.SetParent(Inst.defaultPoolParent);
        obj.transform.position = pos;
        obj.transform.rotation = rot;

        return obj.gameObject;
    }
    public static GameObject Activate(PoolingObj prefab, Transform parent, Vector2 localPos, Quaternion localRot, bool canCreateNew = true)
    {
        PoolingObj obj = ActivateObj(prefab, canCreateNew);
        obj.transform.SetParent(parent);
        obj.transform.localPosition = localPos;
        obj.transform.localRotation = localRot;

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