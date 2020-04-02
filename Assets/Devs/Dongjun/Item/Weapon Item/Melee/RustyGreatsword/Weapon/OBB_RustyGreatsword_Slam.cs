using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_RustyGreatsword_Slam : Weapon_State_Base<OBB_Data_RustyGreatsword, RustyGreatswordItem>,
    ICanDetectGround
{
    [Header("Hit Check")]
    [SerializeField] private Rigidbody2D hitCheck_RB;

    [Header("Visual Effect")]
    [SerializeField] private CameraShake.Data camShakeData_Slam;

    // Hit Check
    private ContactFilter2D contactFilter;
    private OverlapCheckData hitOverlapData;
    private bool hitCheck_Start = false;
    private bool hitCheck_End = false;

    private void Awake()
    {
        contactFilter = new ContactFilter2D { useTriggers = false };

        hitOverlapData = new OverlapCheckData(
            onEnter: overlap =>
            {
                if (overlap.CompareTag("Player"))
                    return;

                PlayerStats.Inst.DealDamage(weaponItem.AttackData_Slam, overlap.gameObject,
                    PlayerActions.WeaponHit,
                    PlayerActions.MeleeSlamHit);
            });
    }

    public override void OnEnter()
    {
        // Animation
        data.Animator.Play("Slam_Airborne");

        // Player
        GM.Player.Data.PlayingOtherMotion = true;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, true);
    }
    public override void OnExit()
    {
        // Hit Check
        hitOverlapData.Clear();
        hitCheck_Start = false;
        hitCheck_End = false;

        // Timer
        weaponItem.Dur_Slam.SetActive(false);
        weaponItem.Dur_Slam.Reset();

        // Animation
        data.Animator.ResetSpeed();

        // Player
        GM.Player.Data.PlayingOtherMotion = false;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, false);
    }
    public override void OnUpdate()
    {
        // Hit Check 0
        if (hitCheck_Start)
        {
            hitCheck_Start = !hitCheck_End;

            List<Collider2D> hits = new List<Collider2D>();
            hitCheck_RB.OverlapCollider(contactFilter, hits);
            hitOverlapData.OverlapCheckOnce(hits);
        }
    }
    public override void OnLateUpdate()
    {
        data.Animator.SetDuration(weaponItem.Dur_Slam.EndTime.Value, "Slam");
    }
    public override void OnFixedUpdate()
    {
        // Detect Ground
        GM.Player.Data.groundDetectionData.DetectGround(true, GM.Player.Data.RB2D, GM.Player.transform);
        GM.Player.Data.groundDetectionData.ExecuteOnGroundMethod(this);

        // Down Vel
        if (!GM.Player.Data.groundDetectionData.isGrounded)
            GM.Player.Data.RB2D.velocity = Vector2.down * weaponItem.SlamDownVel;

        // Player Animation
        UpdatePlayerAnimation();
    }

    private void UpdatePlayerAnimation()
    {
        const string
            Idle = "Idle",
            Airborne = "Airborne";

        GM.Player.Data.Animator.Play(GM.Player.Data.groundDetectionData.isGrounded ? Idle : Airborne);
    }

    private void AnimEvent_Slam_HitCheck_0Start()
    {
        hitCheck_Start = true;
    }
    private void AnimEvent_Slam_HitCheck_0End()
    {
        hitCheck_End = true;
    }

    #region Interface: ICanDetectGround
    void ICanDetectGround.OnGroundEnter()
    {
        // Timer
        weaponItem.Dur_Slam.SetActive(true);

        // Animation
        data.Animator.Play("Slam");

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
}
