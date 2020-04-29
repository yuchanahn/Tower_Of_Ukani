using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GS_Playing : MonoBehaviour, IGameState
{
    private void Awake()
    {
        GS.start_event<GS_Playing>(() => ATimer.SetAndReset(GetInstanceID().ToString(), 1f, () => GS.CurState = FindObjectOfType<GS_GameOver>()));
        GS.transition_event<GS_GameOver, GS_Playing>(()=> { Debug.Log("상태 바뀜."); });
    }

    public void run()
    {
        Debug.Log("playing");
    }
}
