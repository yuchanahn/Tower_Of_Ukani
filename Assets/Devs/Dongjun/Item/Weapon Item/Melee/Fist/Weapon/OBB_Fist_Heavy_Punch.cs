﻿using Dongjun.Helper;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Fist_Heavy_Punch : Weapon_State_Base<OBB_Data_Fist, FistItem>
{
    [Header("Hit Check")]
    [SerializeField] private Rigidbody2D hitCheck_0;

    [Header("Effect")]
    [SerializeField] private SelfSleepObj dustEffect;
    [SerializeField] private CameraShake.Data camShakeData_Punch;

    // Hit Check
    private ContactFilter2D contactFilter;
    private OverlapCheckData hitOverlapData;
    private bool hitCheck_0Start = false;
    private bool hitCheck_0End = false;

    private void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;

        hitOverlapData = new OverlapCheckData(
            onEnter: overlap =>
            {
                if (overlap.CompareTag("Player"))
                    return;

                AttackData attackDataPunch = weaponItem.Heavy_AttackData;
                attackDataPunch.damage 
                    = new FloatStat(baseValue: Mathf.Lerp(1, weaponItem.Heavy_AttackData.damage.Value, weaponItem.Heavy_CurChargeTime / weaponItem.Heavy_FullChargeTime));

                PlayerStats.Inst.DealDamage(attackDataPunch, overlap.gameObject,
                    PlayerActions.WeaponHit,
                    PlayerActions.MeleeHeavyHit);
            });
    }

    public override void OnEnter()
    {
        // Timer
        weaponItem.Heavy_Dur.SetActive(true);

        // Animation
        data.Animator.Play("Heavy_Punch");

        // Effect
        CamShake_Logic.ShakeDir(camShakeData_Punch, transform, transform.right);

        if (GM.Player.Data.groundDetectionData.isGrounded)
            Flip_Logic.FlipXTo(GM.Player.Data.Dir, dustEffect.Spawn(transform.position).transform);

        // Player
        GM.Player.Data.CanDash = false;
        GM.Player.Data.CanKick = false;
        GM.Player.Data.CanChangeDir = false;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, true);
    }
    public override void OnLateEnter()
    {
        data.Animator.SetDuration(weaponItem.Heavy_Dur.EndTime.Value);
    }
    public override void OnExit()
    {
        // Hit Check
        hitOverlapData.Clear();
        hitCheck_0Start = false;
        hitCheck_0End = false;

        // Timer
        weaponItem.Heavy_Dur.SetActive(false);
        weaponItem.Heavy_Dur.Reset();

        // Player
        GM.Player.Data.CanDash = true;
        GM.Player.Data.CanKick = true;
        GM.Player.Data.CanChangeDir = true;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, false);
    }
    public override void OnUpdate()
    {
        // Hit Check 0
        if (hitCheck_0Start)
        {
            hitCheck_0Start = !hitCheck_0End;

            List<Collider2D> hits = new List<Collider2D>();
            hitCheck_0.OverlapCollider(contactFilter, hits);
            hitOverlapData.OverlapCheckOnce(hits);
        }
    }

    private void AnimEvent_Heavy_HitCheck_0Start()
    {
        hitCheck_0Start = true;
    }
    private void AnimEvent_Heavy_HitCheck_0End()
    {
        hitCheck_0End = true;
    }
}
