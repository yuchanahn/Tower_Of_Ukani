using Dongjun.Helper;
using UnityEngine;

public class Fist_Basic : Melee_State_Base<FistItem>
{
    #region Var: Inspector
    [Header("Damage")]
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private Transform damagePoint;
    [SerializeField] private Vector2 damageSize;
    #endregion

    #region Var: Attack
    private int prevAttackAnim = 1;
    #endregion

    #region Prop: 
    public bool IsAnimEnded_Attack { get; private set; } = true;
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(damagePoint.position, damageSize);
    }

    #region Method: SSM
    public override void OnExit()
    {
        IsAnimEnded_Attack = true;

        GM.Player.CanDash = true;
        GM.Player.CanKick = true;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, false);
    }
    public override void OnUpdate()
    {
        if (!weapon.IsSelected)
            return;

        if (PlayerStatus.IsHardCCed || GM.Player.IsKicking)
        {
            weapon.animator.Play(weapon.ANIM_Neutral);
            return;
        }

        if (IsAnimEnded_Attack)
            weapon.animator.Play(weapon.ANIM_Neutral);

        BasicAttack();
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        if (PlayerStatus.IsHardCCed)
            return;

        // Look At Mouse
        transform.LookAtMouseFlipX(Global.Inst.MainCam, transform);
    }
    #endregion

    #region Method: Basic Attack
    private void BasicAttack()
    {
        if (!weapon.basicAttackTimer.IsEnded || !IsAnimEnded_Attack)
            return;

        if (!PlayerWeaponKeys.GetKey(PlayerWeaponKeys.MainAbility))
            return;

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.WeaponMain);
        ActionEffectManager.Trigger(PlayerActions.MeleeBasicAttack);

        // Deal Damage
        var hits = Physics2D.OverlapBoxAll(damagePoint.position, damageSize, 0f, damageLayer);
        for (int i = 0; i < hits.Length; i++)
        {
            // TODO: 무기 힛? 아님 주먹 힛? 아님 둘다?
            PlayerStats.Inst.DealDamage(weapon.attackData, hits[i].gameObject,
                PlayerActions.WeaponHit,
                PlayerActions.MeleeBasicHit);
        }

        // Animation
        IsAnimEnded_Attack = false;
        weapon.animator.Play(prevAttackAnim == 1 ? "Basic_PunchR" : "Basic_PunchL");
        prevAttackAnim *= -1;

        // Player
        GM.Player.CanDash = false;
        GM.Player.CanKick = false;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, true);
    }
    #endregion

    #region Method: Anim Event
    private void OnAnimEnd_Basic()
    {
        // Reset Timer
        weapon.basicAttackTimer.Restart();

        // Animation
        IsAnimEnded_Attack = true;
        weapon.animator.ResetSpeed();

        // Player
        GM.Player.CanDash = true;
        GM.Player.CanKick = true;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, false);
    }
    #endregion
}
