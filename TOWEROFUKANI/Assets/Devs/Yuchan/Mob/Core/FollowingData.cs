using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FollowingData
{
    public bool bFollow;
    public bool bJump;
    public Vector2 nomal;


    public FollowingData(bool bfollow ,bool bjump, Vector2 nom)
    {
        bFollow = bfollow;
        bJump = bjump;
        nomal = nom;
    }
}
