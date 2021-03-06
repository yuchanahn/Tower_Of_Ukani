﻿using Dongjun.Helper;
using System.Collections.Generic;
using UnityEngine;

public class OBB_Fist_Basic : HorizontalWeapon_State_Base<OBB_Data_Fist, FistItem>
{
    [Header("Hit Check")]
    [SerializeField] private Rigidbody2D hitCheck_0;

    [Header("Animation")]
    [SerializeField] private float attackAnimMaxDur = 0;

    // Hit Check
    private ContactFilter2D contactFilter;
    private OverlapCheckData hitOverlapData;

    private bool hitCheck_0Start = false;
    private bool hitCheck_0End = false;

    // Animation
    private int prevAttackAnim = 1;

    private void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;

        hitOverlapData = new OverlapCheckData(
            onEnter: overlap =>
            {
                if (overlap.CompareTag("Player"))
                    return;

                PlayerStats.Inst.DealDamage(weaponItem.AttackData, overlap.gameObject,
                    PlayerActions.WeaponHit,
                    PlayerActions.MeleeBasicHit);
            });
    }

    public override void OnEnter()
    {
        // Timer
        weaponItem.Basic_Dur.SetActive(true);

        // Animation
        data.Animator.Play(prevAttackAnim == 1 ? "Basic_PunchR" : "Basic_PunchL");
        prevAttackAnim *= -1;

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.WeaponMain);
        PlayerActionEventManager.Trigger(PlayerActions.MeleeBasicAttack);

        // Player
        GM.Player.Data.CanDash = false;
        GM.Player.Data.CanKick = false;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, true);
    }
    public override void OnLateEnter()
    {
        data.Animator.SetDuration(weaponItem.Basic_Dur.EndTime.Value, attackAnimMaxDur);
    }
    public override void OnExit()
    {
        // Hit Check
        hitOverlapData.Clear();
        hitCheck_0Start = false;
        hitCheck_0End = false;

        // Timer
        weaponItem.Basic_Dur.SetActive(false);
        weaponItem.Basic_Dur.Reset();

        // Animtaion
        data.Animator.ResetSpeed();

        // Player
        GM.Player.Data.CanDash = true;
        GM.Player.Data.CanKick = true;
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

    private void AnimEvent_Basic_HitCheck_0Start()
    {
        hitCheck_0Start = true;
    }
    private void AnimEvent_Basic_HitCheck_0End()
    {
        hitCheck_0End = true;
    }
}
