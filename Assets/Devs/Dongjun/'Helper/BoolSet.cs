using System.Collections.Generic;
using UnityEngine;

public class BoolSet
{
    private HashSet<object> affectingObjs = new HashSet<object>();
    public bool Value => affectingObjs.Count != 0;

    public void Set<Tthis>(Tthis _this, bool value) where Tthis : class
    {
        if (value && !affectingObjs.Contains(_this))
        {
            affectingObjs.Add(_this);
            return;
        }
        if (!value && affectingObjs.Contains(_this))
        {
            affectingObjs.Remove(_this);
        }
    }
}
public class BoolCount
{
    private int count = 0;
    public bool Value => count != 0;

    public void Set(bool value)
    {
        count += value ? 1 : -1;
        count = Mathf.Max(count, 0);
    }
}
