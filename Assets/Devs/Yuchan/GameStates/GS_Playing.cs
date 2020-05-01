using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GS_Playing : MonoBehaviour, IGameState
{
    private void Awake()
    {
        GS.start_event<GS_Playing>(() => ATimer.SetAndReset(GetInstanceID().ToString(), 1f, () => GS.CurState = FindObjectOfType<GS_GameOver>()));
        GS.transition_event<GS_GameOver, GS_Playing>(()=> { Debug.Log("상태 바뀜."); });
    }


    Text state = null;

    public void run()
    {
        state.IsNull().IF(()=> state = GameObject.Find("GameState").GetComponent<Text>());
        state.text = GetType().Name;
    }
}
