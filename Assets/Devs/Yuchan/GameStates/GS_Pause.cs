using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GS_Pause : MonoBehaviour, IGameState
{
    private void Awake()
    {
        GS.start_event<GS_Pause>(()=> {
            Time.timeScale = 0f;

            GS.SV.무기_에임 = false;
            GS.SV.아이템_드롭_엔드_픽업 = false;
            GS.SV.액티브_사용 = false;
            GS.SV.플레이어_움직임_인풋_허용 = false;
        });
        GS.end_event<GS_Pause>(() => {
            Time.timeScale = 1f;
        });
    }
    public void start_pause()
    {
        GS.CurState = 
            GS.CurState.GetType() == GetType() 
            ? FindObjectOfType<GS_Playing>() as IGameState
            : this as IGameState;
    }
    Text state = null;

    public void run()
    {
        state.IsNull().IF(() => state = GameObject.Find("GameState").GetComponent<Text>());
        state.text = GetType().Name;
    }
}
