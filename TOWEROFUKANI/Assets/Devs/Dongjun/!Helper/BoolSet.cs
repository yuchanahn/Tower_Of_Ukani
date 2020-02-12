using System.Collections;
using System.Collections.Generic;

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
