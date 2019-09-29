using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARandom
{
    public static int Dir => Random.Range(0,2) == 0 ? -1 : 1;
}
