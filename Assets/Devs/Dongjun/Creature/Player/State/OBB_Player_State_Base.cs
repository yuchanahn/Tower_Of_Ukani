using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Player_State_Base : OBB_State, IOBB_State
{
    protected OBB_Data_Player data
    { get; private set; }

    public override void InitData(OBB_Data data)
    {
        this.data = data as OBB_Data_Player;
    }
}
