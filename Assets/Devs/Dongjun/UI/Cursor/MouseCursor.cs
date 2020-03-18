using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }
}
