using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameState
{
    void run();
}


public struct GSValue_t
{
    public bool 무기_에임;
    public bool 아이템_드롭_엔드_픽업;
    public bool 액티브_사용;
    public bool 플레이어_움직임_인풋_허용;
}

public class GS : MonoBehaviour
{
    static Dictionary<Type, Action> events = new Dictionary<Type, Action>();
    static Dictionary<Type, Action> end_events = new Dictionary<Type, Action>();
    static Dictionary<string, Action> tevents = new Dictionary<string, Action>();
    static YCCE<IGameState> cur_state = new YCCE<IGameState>();
    Type prev_type = null;
    internal static IGameState CurState { get => cur_state.Value; set => cur_state.Value = value; }

    public static GSValue_t SV;

    private void Awake()
    {
        // set GS value Defult
        SV.무기_에임 = true;
        SV.아이템_드롭_엔드_픽업 = true;
        SV.액티브_사용 = true;
        SV.플레이어_움직임_인풋_허용 = true;


        cur_state.add_prev_event(()=> { 
            if (cur_state.Value != null)
            {
                end_events.HasKey(CurState.GetType(), () => end_events[CurState.GetType()]());
                prev_type = cur_state.Value.GetType();
            }
        });
        cur_state.add_event(() =>
        {
            events.HasKey(CurState.GetType(), () => events[CurState.GetType()]());
            if (prev_type != null)
            {
                var key = prev_type.Name + CurState.GetType().Name;
                tevents.HasKey(key, ()=>tevents[key]());
            }
        });
    }

    private void Start()
    {
        CurState = FindObjectOfType<GS_Playing>();
    }

    private void Update()
    {
        CurState.run();
    }

    public static void transition_event<T1, T2>(Action f2)
    {
        var key = (typeof(T1).Name + typeof(T2).Name);
        tevents.HasKeyOr(key, () => tevents[key] += f2, () => tevents[key] = f2);
    }

    public static void start_event<T>(Action f2)
    {
        events.HasKeyOr(typeof(T), () => events[typeof(T)] += f2, () => events[typeof(T)] = f2);
    }

    public static void end_event<T>(Action f2)
    {
        end_events.HasKeyOr(typeof(T), () => end_events[typeof(T)] += f2, () => end_events[typeof(T)] = f2);
    }
}
