using System;
using UnityEngine;

public struct IntStat
{
    private bool recalculate;

    // Base Value
    private int baseValue;

    // Cur Min Max
    private int min;
    private int max;

    // Cur Mod
    private int modFlat;
    private float modPercent;

    // Cur Value
    private int value;

    public int BaseMin { get; }
    public int BaseMax { get; }
    public int Base
    {
        get { return baseValue; }
        set { baseValue = Mathf.Clamp(value, min, max); }
    }

    public int Min
    {
        get { return min; }
        set
        {
            min = Math.Min(value, max);
            baseValue = Math.Max(baseValue, min);
            recalculate = true;
        }
    }
    public int Max
    {
        get { return max; }
        set
        {
            max = Math.Max(value, min);
            baseValue = Math.Min(baseValue, max);
            recalculate = true;
        }
    }

    public int ModFlat
    {
        get { return modFlat; }
        set
        {
            modFlat = value;
            recalculate = true;
        }
    }
    public float ModPercent
    {
        get { return modPercent; }
        set
        {
            modPercent = value;
            recalculate = true;
        }
    }

    public int Value
    {
        get
        {
            if (recalculate)
            {
                value = Mathf.Clamp((int)(baseValue * (1 + (modPercent * 0.01f)) + 0.5f) + modFlat, min, max);
                recalculate = false;
            }
            return value;
        }
    }
    public bool IsMin => Value == Min;
    public bool IsMax => Value == Max;

    public IntStat(int baseValue = 0, int min = int.MinValue, int max = int.MaxValue, int modFlat = 0, int modPercent = 0)
    {
        BaseMin = min;
        BaseMax = max;
        this.baseValue = baseValue;

        this.min = min;
        this.max = max;

        this.modFlat = modFlat;
        this.modPercent = modPercent;

        value = 0;
        
        recalculate = true;
    }

    public void Reset()
    {
        Min = BaseMin;
        Max = BaseMax;

        ModFlat = 0;
        ModPercent = 0;
    }
    public void ResetMinMax()
    {
        Min = BaseMin;
        Max = BaseMax;
    }
    public void ResetMod()
    {
        ModFlat = 0;
        ModPercent = 0;
    }
}

public struct FloatStat
{
    private bool recalculate;

    // Base Value
    private float baseValue;

    // Cur Min Max
    private float min;
    private float max;

    // Cur Mod
    private float modFlat;
    private float modPercent;

    // Cur Value
    private float value;

    public float BaseMin { get; }
    public float BaseMax { get; }
    public float Base
    {
        get { return baseValue; }
        set { baseValue = Mathf.Clamp(value, min, max); }
    }

    public float Min
    {
        get { return min; }
        set
        {
            min = Math.Min(value, max);
            baseValue = Math.Max(baseValue, min);
            recalculate = true;
        }
    }
    public float Max
    {
        get { return max; }
        set
        {
            max = Math.Max(value, min);
            baseValue = Math.Min(baseValue, max);
            recalculate = true;
        }
    }

    public float ModFlat
    {
        get { return modFlat; }
        set
        {
            modFlat = value;
            recalculate = true;
        }
    }
    public float ModPercent
    {
        get { return modPercent; }
        set
        {
            modPercent = value;
            recalculate = true;
        }
    }

    public float Value
    {
        get
        {
            if (recalculate)
            {
                value = Mathf.Clamp((baseValue * (1 + modPercent * 0.01f)) + modFlat, min, max);
                recalculate = false;
            }
            return value;
        }
    }
    public bool IsMin => Value == Min;
    public bool IsMax => Value == Max;

    public FloatStat(float baseValue = 0f, float min = float.MinValue, float max = float.MaxValue, float modFlat = 0, float modPercent = 0)
    {
        BaseMin = min;
        BaseMax = max;
        this.baseValue = baseValue;

        this.min = min;
        this.max = max;

        this.modFlat = modFlat;
        this.modPercent = modPercent;

        value = 0;

        recalculate = true;
    }

    public void Reset()
    {
        Min = BaseMin;
        Max = BaseMax;

        ModFlat = 0;
        ModPercent = 0;
    }
    public void ResetMinMax()
    {
        Min = BaseMin;
        Max = BaseMax;
    }
    public void ResetMod()
    {
        ModFlat = 0;
        ModPercent = 0;
    }
}
