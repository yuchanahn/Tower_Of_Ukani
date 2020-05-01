using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GS_GameOver : MonoBehaviour, IGameState
{
    Text state = null;

    public void run()
    {
        state.IsNull().IF(() => state = GameObject.Find("GameState").GetComponent<Text>());
        state.text = GetType().Name;
    }
}
