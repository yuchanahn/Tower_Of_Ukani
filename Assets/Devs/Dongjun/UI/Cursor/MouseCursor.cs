using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;

#if UNITY_STANDALONE && !UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Confined;
#endif
    }
    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }
}
