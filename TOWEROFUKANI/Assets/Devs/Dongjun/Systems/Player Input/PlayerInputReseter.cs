using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputReseter : MonoBehaviour
{
    private void FixedUpdate()
    {
       PlayerInputManager.Inst.ResetInput();
    }
}
