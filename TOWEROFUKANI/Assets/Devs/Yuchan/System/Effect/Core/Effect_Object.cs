using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Object : MonoBehaviour
{
    [SerializeField] Transform AirJumpPos;
    void CreateAirJump(  )
    {
        AirJumpEft.Create(AirJumpPos.position);
    }

    void CreateHitEFT()
    {

    }
}
