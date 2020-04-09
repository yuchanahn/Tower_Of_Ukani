using Dongjun.Helper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB_RustyGreatsword_Basic : Weapon_State_Base<OBB_Data_RustyGreatsword, RustyGreatswordItem>
{
    [Header("Hit Check")]
    [SerializeField] private Rigidbody2D[] hitCheck_RB;

    [Header("Animation")]
    [SerializeField] private float attackAnimMaxDur = 0;

    [Header("Visual Effect")]
    [SerializeField] private CameraShake.Data camShakeData_Attack;

    // Status
    private PlayerStatus_Slow status_Slow;

    // Hit Check
    private ContactFilter2D contactFilter;
    private OverlapCheckData hitOverlapData;
    private bool[] hitCheck_Start;
    private bool[] hitCheck_End;

    private void Awake()
    {
        hitOverlapData = new OverlapCheckData(
            onEnter: overlap =>
            {
                if (overlap.CompareTag("Player"))
                    return;

                PlayerStats.Inst.DealDamage(weaponItem.AttackData, overlap.gameObject,
                    PlayerActions.WeaponHit,
                    PlayerActions.MeleeBasicHit);
            });

        contactFilter = new ContactFilter2D { useTriggers = false };

        hitCheck_Start = new bool[hitCheck_RB.Length];
        hitCheck_End = new bool[hitCheck_RB.Length];
        for (int i = 0; i < hitCheck_RB.Length; i++)
        {
            hitCheck_Start[i] = false;
            hitCheck_End[i] = false;
        }
    }
    private void Start()
    {
        status_Slow = new PlayerStatus_Slow(GM.Player.Data.StatusID, GM.Player.gameObject, 50f);
    }

    public override void OnEnter()
    {
        // Timer
        weaponItem.Dur_Basic.SetActive(true);

        weaponItem.CD_Basic.SetActive(false);
        weaponItem.CD_Basic.Reset();

        // Status Effect
        PlayerStatus.AddEffect(status_Slow);

        // Animation
        data.Animator.Play("Basic");

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.WeaponMain);
        PlayerActionEventManager.Trigger(PlayerActions.MeleeBasicAttack);

        // Player
        GM.Player.Data.CanChangeDir = false;
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
        // Timer
        weaponItem.Dur_Basic.SetActive(false);
        weaponItem.Dur_Basic.Reset();

        weaponItem.CD_Basic.SetActive(true);

        // Status Effect
        PlayerStatus.RemoveEffect(status_Slow);

        // Hit Check
        hitOverlapData.Clear();
        for (int i = 0; i < hitCheck_Start.Length; i++)
        {
            hitCheck_Start[i] = false;
            hitCheck_End[i] = false;
        }

        // Animtaion
        data.Animator.ResetSpeed();

        // Player
        GM.Player.Data.CanChangeDir = true;
        GM.Player.Data.CanDash = true;
        GM.Player.Data.CanKick = true;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, false);
    }
    public override void OnUpdate()
    {
        void HitCheck(int index)
        {
            if (!hitCheck_Start[index])
                return;

            hitCheck_Start[index] = !hitCheck_End[index];
            List<Collider2D> hits = new List<Collider2D>();
            hitCheck_RB[index].OverlapCollider(contactFilter, hits);
            hitOverlapData.OverlapCheckOnce(hits);
        }

        for (int i = 0; i < hitCheck_RB.Length; i++)
            HitCheck(i);
    }

    private void AnimEvent_Basic_HitCheck_0Start()
    {
        hitCheck_Start[0] = true;

        // Effect
        CamShake_Logic.ShakeDir(camShakeData_Attack, transform, Vector2.down);
    }
    private void AnimEvent_Basic_HitCheck_0End()
    {
        hitCheck_End[0] = true;
    }
    private void AnimEvent_Basic_HitCheck_1Start()
    {
        hitOverlapData.Clear();
        hitCheck_Start[1] = true;

        // Effect
        CamShake_Logic.ShakeDir(camShakeData_Attack, transform, Vector2.down);
    }
    private void AnimEvent_Basic_HitCheck_1End()
    {
        hitCheck_End[1] = true;
    }
}
