using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    static GM Inst;

    OBB_Player player;
    [SerializeField] LayerMask soildGround;
    [SerializeField] LayerMask playerLayer;

    public static OBB_Player Player => Inst.player;
    public static LayerMask SoildGroundLayer => Inst.soildGround;
    public static LayerMask PlayerLayer => Inst.playerLayer;
    public static Vector2 PlayerSize => new Vector2(0.5f,0.8f);
    public static GameObject PlayerObj => Inst.player.gameObject;
    public static Vector3 PlayerPos => Inst.player.transform.position;

    [SerializeField] GameObject Map1;
    [SerializeField] GameObject Map2;

    public static Dictionary<string, Texture2D> MapSize = new Dictionary<string, Texture2D>();
    public static Texture2D CurMapSize => MapSize[CurMapName];
    public static string CurMapName;
    public static Vector2 CurMapCenter;
    public static Vector2 CurMapWorldPoint => CurMapCenter + new Vector2((CurMapSize.width / 2) + ((CurMapSize.width % 2 == 0) ? 0.5f : 0), (CurMapSize.height / 2) + ((CurMapSize.height % 2 == 0) ? 0.5f : 0));

    public float pos;


    void Awake()
    {
        Inst = this;
        player = FindObjectOfType<OBB_Player>();
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
