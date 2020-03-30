using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMapData : MonoBehaviour
{
    public void SetCurrentMapName()
    {
        GM.CurMapName = name;
    }
}
