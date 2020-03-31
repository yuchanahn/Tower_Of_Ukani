using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARandom
{
    public static int Dir => Random.Range(0,2) == 0 ? -1 : 1;
    public static Vector2 Dir2d => Random.insideUnitCircle;

    //0~100 %d의 확률로 True False를 반환.
    public static bool Get(int chance) => Random.Range(0, 100) <= chance ? true : false;
    public static bool Get(float chance) => Random.Range(0f, 100f) <= chance ? true : false;
}
