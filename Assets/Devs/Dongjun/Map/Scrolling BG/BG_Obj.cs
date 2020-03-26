using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Obj : MonoBehaviour
{
    [SerializeField] private float speed = 0;

    public float GetSpeed()
    {
        return speed;
    }
}
