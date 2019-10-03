using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct StartPoolData
{
    public PoolingObj prefab;
    public uint initCount;
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
    private static Dictionary<PoolingObj, List<PoolingObj>> pool_Active = new Dictionary<PoolingObj, List<PoolingObj>>();
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
            for (int count = 0; count < startPoolData[i].initCount; count++)
            {
                if (startPoolData[i].prefab == null || startPoolData[i].initCount <= 0)
                    continue;

                PoolingObj prefab = startPoolData[i].prefab;

                if (!pool_Active.ContainsKey(prefab))
                {
                    pool_Active.Add(prefab, new List<PoolingObj>());
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
        if (!pool_Active.ContainsKey(prefab))
        {
            pool_Active.Add(prefab, new List<PoolingObj>());
            pool_Sleeping.Add(prefab, new Queue<PoolingObj>());
            pool_Sleeping[prefab].Enqueue(Instantiate(prefab));
        }

        PoolingObj obj = pool_Sleeping[prefab].Count != 0 ? pool_Sleeping[prefab].Dequeue() : canCreateNew ? Instantiate(prefab) : pool_Active[prefab][0];
        obj.InitPoolingObj(prefab);

        pool_Active[prefab].Remove(obj);
        pool_Active[prefab].Add(obj);

        obj.gameObject.SetActive(true);

        return obj;
    }
    #endregion

    #region Method: Public
    public static void SetUpObjPool(StartPoolData startPoolData)
    {
        if (startPoolData.prefab == null)
            return;

        for (int count = 0; count < startPoolData.initCount; count++)
        {
            if (startPoolData.prefab == null || startPoolData.initCount <= 0)
                continue;

            PoolingObj prefab = startPoolData.prefab;

            if (!pool_Active.ContainsKey(prefab))
            {
                pool_Active.Add(prefab, new List<PoolingObj>());
                pool_Sleeping.Add(prefab, new Queue<PoolingObj>());
            }

            PoolingObj obj = Instantiate(prefab);
            obj.InitPoolingObj(prefab);

            pool_Sleeping[prefab].Enqueue(obj);

            obj.transform.SetParent(Inst.defaultPoolParent);
            obj.gameObject.SetActive(false);
        }
    }
    public static GameObject Spawn(PoolingObj prefab, Vector2 pos, Quaternion rot, bool canCreateNew = true)
    {
        PoolingObj obj = ActivateObj(prefab, canCreateNew);
        obj.transform.SetParent(Inst.defaultPoolParent);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.ResetOnActive();

        return obj.gameObject;
    }
    public static GameObject Spawn(PoolingObj prefab, Transform parent, Vector2 localPos, Quaternion localRot, bool canCreateNew = true)
    {
        PoolingObj obj = ActivateObj(prefab, canCreateNew);
        obj.transform.SetParent(parent);
        obj.transform.localPosition = localPos;
        obj.transform.localRotation = localRot;
        obj.ResetOnActive();

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

public static class ObjPoolingExtension
{
    public static GameObject Spawn(this PoolingObj prefab, Vector2 pos, Quaternion rot, bool canCreateNew = true)
    {
        return ObjPoolingManager.Spawn(prefab, pos, rot, canCreateNew);
    }
    public static GameObject Spawn(this PoolingObj prefab, Transform parent, Vector2 localPos, Quaternion localRot, bool canCreateNew = true)
    {
        return ObjPoolingManager.Spawn(prefab, parent, localPos, localRot, canCreateNew);
    }
    public static void Sleep(this PoolingObj obj)
    {
        ObjPoolingManager.Sleep(obj);
    }
}