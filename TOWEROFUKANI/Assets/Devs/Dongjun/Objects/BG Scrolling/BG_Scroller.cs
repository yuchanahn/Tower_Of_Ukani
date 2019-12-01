using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Scroller : MonoBehaviour
{
    [SerializeField] private float globalSpeed = 0.01f;
    [SerializeField] private BG_Obj[] bgObjects;
    private Material[] materials;

    float camPosX_Cur;
    float camPosX_Old;
    float deltaLength;

    private void Awake()
    {
        camPosX_Cur = transform.position.x;
        camPosX_Old = camPosX_Cur;

        materials = new Material[bgObjects.Length];

        for (int i = 0; i < bgObjects.Length; i++)
        {
            materials[i] = bgObjects[i].GetComponent<MeshRenderer>().material;
        }
    }
    private void LateUpdate()
    {
        Scroll();
    }

    private void Scroll()
    {
        camPosX_Cur = transform.position.x;
        deltaLength = (camPosX_Cur - camPosX_Old) * globalSpeed;

        for (int i = 0; i < bgObjects.Length; i++)
        {
            materials[i].mainTextureOffset = new Vector2(deltaLength * bgObjects[i].GetSpeed(), 0);
        }
    }
}
