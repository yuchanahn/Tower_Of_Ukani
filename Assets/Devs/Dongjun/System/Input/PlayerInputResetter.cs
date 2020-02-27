using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputResetter : MonoBehaviour
{
    private void FixedUpdate()
    {
        PlayerInputManager.Inst.ResetInput();
    }
}
