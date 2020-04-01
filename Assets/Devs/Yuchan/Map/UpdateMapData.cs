using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMapData : MonoBehaviour
{
    [SerializeField] Vector2 MapCenter;
    public void SetCurrentMapName()
    {
        GM.CurMapName = name;
        GM.CurMapCenter = MapCenter;
    }
}
