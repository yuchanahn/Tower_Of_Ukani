using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IGameState
{
    void run();
}

public class GS : MonoBehaviour
{
    static Dictionary<Type, Action> events = new Dictionary<Type, Action>();
    static Dictionary<string, Action> tevents = new Dictionary<string, Action>();
    static YCCE<IGameState> cur_state = new YCCE<IGameState>();
    Type pt = null;
    internal static IGameState CurState { get => cur_state.Value; set => cur_state.Value = value; }

    private void Awake()
    {
        cur_state.add_prev_event(()=> { if(cur_state.Value != null) pt = cur_state.Value.GetType(); });
        cur_state.add_event(() =>
        {
            events.HasKey(CurState.GetType(), () => events[CurState.GetType()]());
            if (pt != null)
            {
                var key = pt.Name + CurState.GetType().Name;
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
}
