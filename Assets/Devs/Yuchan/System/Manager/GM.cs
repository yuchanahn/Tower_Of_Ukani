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

    public static OBB_Player Player => Inst.player;
    public static LayerMask SoildGroundLayer => Inst.soildGround;
    public static LayerMask PlayerLayer => Inst.playerLayer;
    public static Vector2 PlayerSize => new Vector2(0.5f, 0.8f);
    public static GameObject PlayerObj => Inst.player.gameObject;
    public static Vector3 PlayerPos => Inst.player.transform.position;

    [SerializeField] GameObject Map1;
    [SerializeField] GameObject Map2;


    

    public static Dictionary<string, Texture2D> MapSize = new Dictionary<string, Texture2D>();
    [SerializeField] _mapsize_data_raw[] MapSizeDefult;
    public static Dictionary<string, Vector2Int> MapSizeOf = new Dictionary<string, Vector2Int>();


    public static Texture2D CurMapSize => MapSize[CurMapName];
    
    public static int CurMapSize_Width => MapSizeOf[CurMapName].x;
    public static int CurMapSize_Heigth => MapSizeOf[CurMapName].y;



    public static string CurMapName;
    public static Vector2 CurMapCenter;
    public static Vector2 CurMapWorldPoint => CurMapCenter + new Vector2((CurMapSize.width / 2) + ((CurMapSize.width % 2 == 0) ? 0.5f : 0), (CurMapSize.height / 2) + ((CurMapSize.height % 2 == 0) ? 0.5f : 0));

    public float pos;


    void Awake()
    {
        Inst = this;
        player = FindObjectOfType<OBB_Player>();

        MapSizeDefult.for_each( x => MapSizeOf[x.MapName] = x.MapSize );

        CurMapName = Map2.activeSelf ? Map2.name : Map1.name;
    }

    private void Update()
    {
        CurMapName = Map2.activeSelf ? Map2.name : Map1.name;
    }


    private void OnGUI()
    {
        //if(GUI.Button(new Rect(10,20, 100,20), "on"))
        {
        }
    }
}
