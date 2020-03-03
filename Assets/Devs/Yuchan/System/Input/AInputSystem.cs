using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





 
public enum eKeyID
{
    Player_Attack,
    UI_Open
}



[System.Serializable]
public struct AINPUT
{
    public KeyCode Key;
    public eKeyID MappingTo;
}

public static class IN
{
    private static Dictionary<eKeyID, KeyCode> keys = new Dictionary<eKeyID, KeyCode>();

    public static System.Action PrevSetEvent;
    public static System.Action SetEvent;

    public static Dictionary<eKeyID, KeyCode> Keys { get => keys; set { PrevSetEvent();  keys = value; SetEvent(); } }

    public static KeyCode PlayerAttack => Keys[eKeyID.Player_Attack];
    public static bool Down(this eKeyID k) => Input.GetKeyDown(Keys[k]);
    public static bool Up(this eKeyID k) => Input.GetKeyUp(Keys[k]);
    public static bool Get(this eKeyID k) => Input.GetKey(Keys[k]);
}

public class AInputSystem : MonoBehaviour
{
    [SerializeField] AINPUT[] DefaultKeys = null;

    private void Start()
    {
        foreach(var i in DefaultKeys)
        {
            IN.Keys[i.MappingTo] = i.Key;
        }
    }

    private void Update()
    {
    }
}
