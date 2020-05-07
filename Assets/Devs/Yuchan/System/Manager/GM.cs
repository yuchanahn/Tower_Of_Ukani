using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class _mapsize_data_raw
{
    public string MapName;
    public Vector2Int MapSize;
}

public class GM : MonoBehaviour
{
    static GM Inst;

    OBB_Player player;
    [SerializeField] LayerMask soildGround;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] int[] JPS_UseableObjectSizes;
    [SerializeField] AnimationCurve test_cur;

    public static OBB_Player Player => Inst.player;
    public static LayerMask SoildGroundLayer => Inst.soildGround;
    public static LayerMask PlayerLayer => Inst.playerLayer;
    public static Vector2 PlayerSize => new Vector2(0.5f, 0.8f);
    public static GameObject PlayerObj => Inst.player.gameObject;
    public static Vector3 PlayerPos => Inst.player.transform.position;
    public static int[] JPSObjSizes => Inst.JPS_UseableObjectSizes;


    [SerializeField] _mapsize_data_raw[] MapSizeDefult;
    public static Dictionary<string, Vector2Int> MapSizeOf = new Dictionary<string, Vector2Int>();
    public static int CurMapSize_Width => MapSizeOf[CurMapName].x;
    public static int CurMapSize_Height => MapSizeOf[CurMapName].y;

    public static string CurMapName;
    public static AnimationCurve testcur;
    public static Vector2 CurMapCenter;
    public static Vector2 CurMapWorldPoint => CurMapCenter + new Vector2((CurMapSize_Width / 2) + ((CurMapSize_Width % 2 == 0) ? 0.5f : 0), (CurMapSize_Height / 2) + ((CurMapSize_Height % 2 == 0) ? 0.5f : 0));

    public float pos;

    [SerializeField] public GameObject test_mark;
    [SerializeField] public GameObject test_mark2;
    static public GameObject test_mark_;
    static public GameObject test_mark_2;

    void Awake()
    {
        Inst = this;
        player = FindObjectOfType<OBB_Player>();
        test_mark_ = test_mark;
        test_mark_2 = test_mark2;
        MapSizeDefult.for_each( x => MapSizeOf[x.MapName] = x.MapSize );
        testcur = test_cur;
    }

    private void OnGUI()
    {
        //if(GUI.Button(new Rect(10,20, 100,20), "on"))
        {
        }
    }
}
