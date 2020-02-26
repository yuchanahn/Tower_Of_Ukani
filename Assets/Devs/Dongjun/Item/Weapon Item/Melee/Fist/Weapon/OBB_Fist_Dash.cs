using System.Collections.Generic;
using UnityEngine;

public class OBB_Fist_Dash : Weapon_State_Base<OBB_Data_Fist, FistItem>
{
    [Header("Hit Check")]
    [SerializeField] private Rigidbody2D hitCheck_0;

    // Hit Check
    private ContactFilter2D contactFilter;
    private OverlapCheckData hitOverlapData;

    private void Awake()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = false;

        // Deal Damage
        hitOverlapData = new OverlapCheckData(
            onEnter: overlap =>
            {
                PlayerStats.Inst.DealDamage(weaponItem.AttackData_Dash, overlap.gameObject,
                    PlayerActions.WeaponHit,
                    PlayerActions.MeleeDashHit);
            });
    }

    public override void OnEnter()
    {
        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.MeleeDashAttack);

        // Look Dir
        Flip_Logic.FlipXTo(GM.Player.Data.Dir, transform);

        // Animation
        data.Animator.Play("Dash");
    }
    public override void OnExit()
    {
        // Hit Check
        hitOverlapData.Clear();
    }
    public override void OnUpdate()
    {
        // Hit Check 0
        List<Collider2D> hits = new List<Collider2D>();
        hitCheck_0.OverlapCollider(contactFilter, hits);
        hitOverlapData.OverlapCheckOnce(hits);
    }
}
