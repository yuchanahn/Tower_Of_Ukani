using Dongjun.Helper;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionEventManager : MonoBehaviour
{
    private static Dictionary<PlayerActions, List<PlayerActionEvent>> events;

    private void Awake()
    {
        // Init Actions Dictionary
        events = new Dictionary<PlayerActions, List<PlayerActionEvent>>();

        // Init Keys
        for (int i = 0; i < EnumHelper.Count<PlayerActions>(); i++)
            events.Add((PlayerActions)i, new List<PlayerActionEvent>());
    }

    public static void Trigger(PlayerActions action)
    {
        for (int i = 0; i < events[action].Count; i++)
            events[action][i].EffectAction?.Invoke();
    }

    public static void AddEvent(PlayerActions action, PlayerActionEvent actionEvent)
    {
        if (events[action].Contains(actionEvent))
            return;

        int index = events[action].FindIndex(i => actionEvent.ThisType == i.AfterThis);
        if (index != -1)
        {
            events[action].Insert(index, actionEvent);
            return;
        }

        events[action].Add(actionEvent);
    }
    public static void RemoveEvent(PlayerActions action, PlayerActionEvent actionEvent)
    {
        if (events[action].Contains(actionEvent))
            events[action].Remove(actionEvent);
    }
}
