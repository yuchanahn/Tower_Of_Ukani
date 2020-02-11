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

    #region Var: Properties
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
        OnAnimEnd_Attack();
    }
    public override void OnUpdate()
    {
        if (!weapon.IsSelected)
            return;

        if (PlayerStatus.Inst.IsStunned)
        {
            OnAnimEnd_Attack();
            weapon.animator.Play(weapon.ANIM_Neutral);
            return;
        }

        if (IsAnimEnded_Attack)
            weapon.animator.Play(weapon.ANIM_Neutral);

        BasicAttack();
    }
    public override void OnLateUpdate()
    {
        if (PlayerStatus.Inst.IsStunned || !weapon.IsSelected)
            return;

        // Look At Mouse
        transform.LookAtMouseFlipX(Global.Inst.MainCam, transform);
    }
    #endregion

    #region Method: Basic Attack
    private void BasicAttack()
    {
        if (!weapon.basicAttackTimer.IsEnded || !IsAnimEnded_Attack || !PlayerWeaponKeys.GetKey(PlayerWeaponKeys.MainAbility))
            return;

        // Deal Damage
        var hits = Physics2D.OverlapBoxAll(damagePoint.position, damageSize, 0f, damageLayer);
        for (int i = 0; i < hits.Length; i++)
        {
            PlayerStats.Inst.DealDamage(new AttackData(2), hits[i].gameObject);
        }

        // Lock Weapon Slot
        PlayerInventoryManager.weaponHotbar.LockChangeSlot(true);

        // Animation
        IsAnimEnded_Attack = false;
        weapon.animator.Play(prevAttackAnim == 1 ? "Basic_PunchR" : "Basic_PunchL");
        prevAttackAnim *= -1;

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.WeaponMain);
        ActionEffectManager.Trigger(PlayerActions.GunShoot);
    }
    #endregion

    #region Method: Anim Event
    private void OnAnimEnd_Attack()
    {
        weapon.basicAttackTimer.Restart();

        // Unlock Weapon Slot
        if (!IsAnimEnded_Attack)
            PlayerInventoryManager.weaponHotbar.LockChangeSlot(false);

        IsAnimEnded_Attack = true;
        weapon.animator.ResetSpeed();
    }
    #endregion
}
