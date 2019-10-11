using System;
using UnityEngine;
using NaughtyAttributes;

[Serializable]
public struct IntStat
{
    public int baseStat;
    [HideInInspector] public int curStat;

    [HideInInspector] public float percentAdd;
    [HideInInspector] public float flatAdd;

    public void Init()
    {
        curStat = baseStat;

        percentAdd = 0;
        flatAdd = 0;
    }

    public void ApplyAddedStat()
    {
        curStat = (int)(baseStat * (1 + (percentAdd / 100)) + flatAdd + 0.5f);
    }
}

[Serializable]
public struct FloatStat
{
    public float baseStat;
    [HideInInspector] public float curStat;

    [HideInInspector] public float percentAdd;
    [HideInInspector] public float flatAdd;

    public void Init()
    {
        curStat = baseStat;

        percentAdd = 0;
        flatAdd = 0;
    }

    public void ApplyAddedStat()
    {
        curStat = baseStat * (1 + (percentAdd / 100)) + flatAdd;
    }
}
