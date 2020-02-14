using UnityEngine;

public class Fist_Slam : Melee_State_Base<FistItem>,
     ICanDetectGround
{
    #region Var: Inspector
    [Header("Damage")]
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private Transform damagePoint;
    [SerializeField] private Vector2 damageSize;

    [Header("Effect")]
    [SerializeField] private CameraShake.Data camShakeData_Slam;
    #endregion

    [HideInInspector] public bool slamAnimEnd = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(damagePoint.position, damageSize);
    }

    public override void OnEnter()
    {
        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.MeleeSlamAttack);

        // Animation
        weapon.animator.Play("Slam_Airborne");

        GM.Player.PlayingOtherMotion = true;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, true);
    }
    public override void OnExit()
    {
        slamAnimEnd = false;

        GM.Player.PlayingOtherMotion = false;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, false);
    }
    public override void OnFixedUpdate()
    {
        // Down Vel
        GM.Player.RB2D.velocity = Vector2.down * weapon.slamDownVel;

        // Detect Ground
        GM.Player.groundDetectionData.DetectGround(true, GM.Player.RB2D, GM.Player.transform);
        GM.Player.groundDetectionData.ExecuteOnGroundMethod(this);
    }

    #region Interface: ICanDetectGround
    void ICanDetectGround.OnGroundEnter()
    {
        // Deal Damage
        var hits = Physics2D.OverlapBoxAll(damagePoint.position, damageSize, 0f, damageLayer);
        for (int i = 0; i < hits.Length; i++)
        {
            // TODO: 무기 힛? 아님 주먹 힛? 아님 둘다?
            PlayerStats.Inst.DealDamage(weapon.attackData_Slam, hits[i].gameObject,
                PlayerActions.WeaponHit,
                PlayerActions.MeleeSlamHit);
        }

        // Animation
        weapon.animator.Play("Slam");

        // Effect
        CamShake_Logic.ShakeDir(camShakeData_Slam, transform, Vector2.down);
    }
    void ICanDetectGround.OnGroundStay()
    {
    }
    void ICanDetectGround.OnGroundExit()
    {
    }
    #endregion

    #region Method: Anim Event
    private void OnAnimEnd_Slam()
    {
        slamAnimEnd = true;
    }
    #endregion
}
