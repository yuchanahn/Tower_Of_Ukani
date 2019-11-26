using System;
using UnityEngine;

[Serializable]
public struct IntStat
{
    private bool needToCalculate;

    private int @base;
    private int min;
    private int max;

    private int modFlat;
    private float modPercent;

    private int value;

    public int Base
    {
        get { return @base; }
        set { @base = Mathf.Clamp(value, min, max); }
    }
    public int Min
    {
        get { return min; }
        set
        {
            min = Math.Min(value, max);
            @base = Math.Max(@base, min);
            needToCalculate = true;
        }
    }
    public int Max
    {
        get { return max; }
        set
        {
            max = Math.Max(value, min);
            @base = Math.Min(@base, max);
            needToCalculate = true;
        }
    }

    public int ModFlat
    {
        get { return modFlat; }
        set
        {
            modFlat = value;
            needToCalculate = true;
        }
    }
    public float ModPercent
    {
        get { return modPercent; }
        set
        {
            modPercent = value;
            needToCalculate = true;
        }
    }

    public int Value
    {
        get
        {
            if (needToCalculate)
            {
                value = Mathf.Clamp((int)(@base * (1 + (modPercent * 0.01f)) + 0.5f) + modFlat, min, max);
                needToCalculate = false;
            }
            return value;
        }
    }

    public IntStat(int @base = 0, int min = int.MinValue, int max = int.MaxValue, int modFlat = 0, int modPercent = 0)
    {
        this.needToCalculate = true;
        this.@base = @base;
        this.min = min;
        this.max = max;
        this.modFlat = modFlat;
        this.modPercent = modPercent;
        this.value = 0;
    }
}

[Serializable]
public struct FloatStat
{
    private bool needToCalculate;

    private float @base;
    private float min;
    private float max;

    private float modFlat;
    private float modPercent;

    private float value;

    public float Base
    {
        get { return @base; }
        set { @base = Mathf.Clamp(value, min, max); }
    }
    public float Min
    {
        get { return min; }
        set
        {
            min = Math.Min(value, max);
            @base = Math.Max(@base, min);
            needToCalculate = true;
        }
    }
    public float Max
    {
        get { return max; }
        set
        {
            max = Math.Max(value, min);
            @base = Math.Min(@base, max);
            needToCalculate = true;
        }
    }

    public float ModFlat
    {
        get { return modFlat; }
        set
        {
            modFlat = value;
            needToCalculate = true;
        }
    }
    public float ModPercent
    {
        get { return modPercent; }
        set
        {
            modPercent = value;
            needToCalculate = true;
        }
    }

    public float Value
    {
        get
        {
            if (needToCalculate)
            {
                value = Mathf.Clamp((@base * (1 + modPercent * 0.01f)) + modFlat, min, max);
                needToCalculate = false;
            }
            return value;
        }
    }

    public FloatStat(float @base = 0f, float min = float.MinValue, float max = float.MaxValue, float modFlat = 0, float modPercent = 0)
    {
        this.needToCalculate = true;
        this.@base = @base;
        this.min = min;
        this.max = max;
        this.modFlat = modFlat;
        this.modPercent = modPercent;
        this.value = 0;
    }
}
