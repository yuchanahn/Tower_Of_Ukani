using Dongjun.Helper;
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
        weaponItem.Dur_Basic.SetActive(true);

        // Animation
        data.Animator.Play(prevAttackAnim == 1 ? "Basic_PunchR" : "Basic_PunchL");
        prevAttackAnim *= -1;

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.WeaponMain);
        ActionEffectManager.Trigger(PlayerActions.MeleeBasicAttack);

        // Player
        GM.Player.CanDash = false;
        GM.Player.CanKick = false;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, true);
    }
    public override void OnLateEnter()
    {
        data.Animator.SetDuration(weaponItem.Dur_Basic.EndTime.Value, attackAnimMaxDur);
    }
    public override void OnExit()
    {
        // Hit Check
        hitOverlapData.Clear();
        hitCheck_0Start = false;
        hitCheck_0End = false;

        // Timer
        weaponItem.Dur_Basic.SetActive(false);
        weaponItem.Dur_Basic.Reset();

        // Animtaion
        data.Animator.ResetSpeed();

        // Player
        GM.Player.CanDash = true;
        GM.Player.CanKick = true;
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

            for (int i = 0; i < hits.Count; i++)
            {
                Debug.Log(hits[i].gameObject.name);
            }
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
