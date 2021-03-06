﻿using Dongjun.Helper;
using System.Collections.Generic;
using UnityEngine;

public abstract class PoolingObj : MonoBehaviour
{
    public PoolingObj Prefab { get; private set; }
    
    public void InitPoolingObj(PoolingObj prefab)
    {
        Prefab = prefab;
    }
    public abstract void OnSpawn();
}

public class ObjPoolingManager : SingletonBase<ObjPoolingManager>
{
    #region Struct: Start Pool Data
    [System.Serializable]
    public struct StartPoolData
    {
        public PoolingObj prefab;
        public uint initCount;
    }
    #endregion

    #region Var: Inspector
    [SerializeField, Header("Default Parent Obj of Pooling Objects")]
    private Transform defaultPoolParent;

    [SerializeField, Header("Spawn Object On Start")]
    private StartPoolData[] startPoolData;
    #endregion

    #region Var: Pool Data
    private static Dictionary<PoolingObj, List<PoolingObj>> pool_Active;
    private static Dictionary<PoolingObj, Queue<PoolingObj>> pool_Sleeping;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        pool_Active = new Dictionary<PoolingObj, List<PoolingObj>>();
        pool_Sleeping = new Dictionary<PoolingObj, Queue<PoolingObj>>();

        SetUpStartPool();
    }
    #endregion

    #region Method: Private
    private void SetUpStartPool()
    {
        if (startPoolData is null)
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
    private static T ActivateObj<T>(T prefab, bool canCreateNew) where T : PoolingObj
    {
        if (!pool_Active.ContainsKey(prefab))
        {
            pool_Active.Add(prefab, new List<PoolingObj>());
            pool_Sleeping.Add(prefab, new Queue<PoolingObj>());
            pool_Sleeping[prefab].Enqueue(Instantiate(prefab));
        }

        T obj = (pool_Sleeping[prefab].Count != 0 ? pool_Sleeping[prefab].Dequeue() : canCreateNew ? Instantiate(prefab) : pool_Active[prefab][0]) as T;
        obj.InitPoolingObj(prefab);

        pool_Active[prefab].Remove(obj);
        pool_Active[prefab].Add(obj);
        return obj;
    }
    private static void InitObj<T>(T obj, Vector3 pos, Quaternion rot) where T : PoolingObj
    {
        obj.transform.SetParent(Inst.defaultPoolParent);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
    }
    private static void InitObj<T>(T obj, Transform parent, Vector2 localPos, Quaternion localRot) where T : PoolingObj
    {
        obj.transform.SetParent(parent);
        obj.transform.localPosition = localPos;
        obj.transform.localRotation = localRot;
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
    }
    #endregion

    #region Method: Public
    public static void SetUpObjPool(StartPoolData startPoolData)
    {
        if (startPoolData.prefab == null)
            return;

        for (int count = 0; count < startPoolData.initCount; count++)
        {
            if (startPoolData.prefab is null || startPoolData.initCount <= 0)
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

    public static T Spawn<T>(T prefab, Vector2 pos, Quaternion rot, bool canCreateNew = true) where T : PoolingObj
    {
        T obj = ActivateObj(prefab, canCreateNew);
        InitObj(obj, pos, rot);
        return obj;
    }
    public static T Spawn<T>(T prefab, Vector2 pos, bool canCreateNew = true) where T : PoolingObj
    {
        T obj = ActivateObj(prefab, canCreateNew);
        InitObj(obj, pos, Quaternion.identity);
        return obj;
    }
    public static T Spawn<T>(T prefab, Transform parent, Vector2 localPos, Quaternion localRot, bool canCreateNew = true) where T : PoolingObj
    {
        T obj = ActivateObj(prefab, canCreateNew);
        InitObj(obj, parent, localPos, localRot);
        return obj;
    }
    public static T Spawn<T>(T prefab, Transform parent, Vector2 localPos, bool canCreateNew = true) where T : PoolingObj
    {
        T obj = ActivateObj(prefab, canCreateNew);
        InitObj(obj, parent, localPos, Quaternion.identity);
        return obj;
    }

    public static void Sleep(PoolingObj obj)
    {
        if (obj.Prefab != null && pool_Active.ContainsKey(obj.Prefab))
        {
            if (pool_Active[obj.Prefab].Contains(obj))
            {
                pool_Active[obj.Prefab].Remove(obj);
                pool_Sleeping[obj.Prefab].Enqueue(obj);
                obj.gameObject.SetActive(false);
                return;
            }
            return;
        }

        // Destroy if there is no Active PoolingObj
        if (!obj.gameObject.IsPrefab())
            Destroy(obj.gameObject);
    }
    #endregion
}

public static class ObjPoolingExtension
{
    public static T Spawn<T>(this T prefab, Vector2 pos, Quaternion rot, bool canCreateNew = true) where T : PoolingObj
    {
        return ObjPoolingManager.Spawn(prefab, pos, rot, canCreateNew);
    }
    public static T Spawn<T>(this T prefab, Vector2 pos, bool canCreateNew = true) where T : PoolingObj
    {
        return ObjPoolingManager.Spawn(prefab, pos, canCreateNew);
    }
    public static T Spawn<T>(this T prefab, Transform parent, Vector2 localPos, Quaternion localRot, bool canCreateNew = true) where T : PoolingObj
    {
        return ObjPoolingManager.Spawn(prefab, parent, localPos, localRot, canCreateNew);
    }
    public static T Spawn<T>(this T prefab, Transform parent, Vector2 localPos, bool canCreateNew = true) where T : PoolingObj
    {
        return ObjPoolingManager.Spawn(prefab, parent, localPos, canCreateNew);
    }

    public static void Sleep(this PoolingObj obj)
    {
        ObjPoolingManager.Sleep(obj);
    }
}