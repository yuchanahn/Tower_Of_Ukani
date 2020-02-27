using UnityEngine;

public class Player_Effect_Object : MonoBehaviour
{
    [SerializeField] Transform feetPos;

    void CreateAirJump()
    {
        AirJumpEft.Create(feetPos.position);
    }
    void CreateWalkDust()
    {
        var obj = PlayerWalkDustEft.Create(feetPos.position);
        obj.GetComponentInChildren<SpriteRenderer>().flipX = PlayerInputManager.Inst.Input_WalkDir >= 0 ? false : true;
    }
}
