using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonObjs : MonoBehaviour
{
    public static CommonObjs Inst;

    [SerializeField] private Camera mainCam;

    public Camera MainCam => mainCam;

    private void Awake()
    {
        Inst = this;
    }
}
