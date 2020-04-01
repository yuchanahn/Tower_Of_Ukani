using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_RustyGreatsword_Basic : Weapon_State_Base<OBB_Data_RustyGreatsword, RustyGreatswordItem>
{
    [Header("Hit Check")]
    [SerializeField] private Rigidbody2D hitCheck_0;
    [SerializeField] private Rigidbody2D hitCheck_1;

    [Header("Animation")]
    [SerializeField] private float attackAnimMaxDur = 0;

    private PlayerStatus_Slow status_Slow;

    // Hit Check
    private ContactFilter2D contactFilter;
    private OverlapCheckData hitOverlapData;

    private bool hitCheck_0Start = false;
    private bool hitCheck_0End = false;

    private bool hitCheck_1Start = false;
    private bool hitCheck_1End = false;

    private void Awake()
    {
        status_Slow = new PlayerStatus_Slow(GM.Player.Data.StatusID, GM.Player.gameObject, 50f);

        contactFilter = new ContactFilter2D
        {
            useTriggers = false
        };

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

        // Status Effect
        PlayerStatus.AddEffect(status_Slow);

        // Animation
        data.Animator.Play("Basic");

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
        data.Animator.SetDuration(weaponItem.Dur_Basic.EndTime.Value, attackAnimMaxDur);
    }
    public override void OnExit()
    {
        // Hit Check
        hitOverlapData.Clear();
        hitCheck_0Start = false;
        hitCheck_0End = false;
        hitCheck_1Start = false;
        hitCheck_1End = false;

        // Timer
        weaponItem.Dur_Basic.SetActive(false);
        weaponItem.Dur_Basic.Reset();

        // Status Effect
        PlayerStatus.RemoveEffect(status_Slow);

        // Animtaion
        data.Animator.ResetSpeed();

        // Player
        GM.Player.Data.CanDash = true;
        GM.Player.Data.CanKick = true;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, false);
    }
    public override void OnUpdate()
    {
        void HitCheck(ref bool start, bool end)
        {
            if (start)
            {
                start = !end;
                List<Collider2D> hits = new List<Collider2D>();
                hitCheck_0.OverlapCollider(contactFilter, hits);
                hitOverlapData.OverlapCheckOnce(hits);
            }
        }

        HitCheck(ref hitCheck_0Start, hitCheck_0End);
        HitCheck(ref hitCheck_1Start, hitCheck_1End);
    }

    private void AnimEvent_Basic_HitCheck_0Start()
    {
        hitCheck_0Start = true;
    }
    private void AnimEvent_Basic_HitCheck_0End()
    {
        hitCheck_0End = true;
    }
    private void AnimEvent_Basic_HitCheck_1Start()
    {
        hitOverlapData.Clear();
        hitCheck_1Start = true;
    }
    private void AnimEvent_Basic_HitCheck_1End()
    {
        hitCheck_1End = true;
    }
}
