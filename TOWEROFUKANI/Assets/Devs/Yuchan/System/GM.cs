using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    static GM Inst;

    Player player;

    public static Player Player => Inst.player;
    public static Vector2 PlayerSize => new Vector2(0.5f,0.8f);
    public static GameObject PlayerObj => Inst.player.gameObject;
    public static Vector3 PlayerPos => Inst.player.transform.position;


    void Awake()
    {
        Inst = this;
        player = FindObjectOfType<Player>();
    }
}
