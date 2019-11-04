using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLA_ActionBase<TMain> : CLA_Action 
    where TMain : CLA_Main
{
    protected TMain main;

    protected virtual void Awake()
    {
        main = GetComponent<TMain>();
    }
}
