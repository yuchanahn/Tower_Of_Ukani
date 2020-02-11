using UnityEngine;

public class Fist_Heavy : Melee_State_Base<FistItem>
{
    #region Var: Inspector
    [Header("Damage")]
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private Transform damagePoint;
    [SerializeField] private Vector2 damageSize;
    #endregion

    private bool chargeDone = false;
    private float chargePower = 0;

    public bool PunchAnimEnd { get; private set; } = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(damagePoint.position, damageSize);
    }

    public override void OnEnter()
    {
        PlayerInventoryManager.weaponHotbar.LockChangeSlot(true);
        GM.Player.CanDash = false;
        GM.Player.CanKick = false;
    }
    public override void OnExit()
    {
        chargeDone = false;
        chargePower = 0;
        PunchAnimEnd = false;

        PlayerInventoryManager.weaponHotbar.LockChangeSlot(false);
        GM.Player.CanDash = true;
        GM.Player.CanKick = true;
    }
    public override void OnUpdate()
    {
        if (PlayerStatus.Inst.IsStunned || !weapon.IsSelected)
            return;

        if (!chargeDone)
        {
            chargePower += Time.deltaTime;
            weapon.animator.Play("Heavy_Charge");
        }

        if (Input.GetKeyUp(PlayerWeaponKeys.SubAbility))
        {
            chargeDone = true;
            weapon.animator.Play("Heavy_Punch");

            // Deal Damage
            var hits = Physics2D.OverlapBoxAll(damagePoint.position, damageSize, 0f, damageLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                PlayerStats.Inst.DealDamage(new AttackData(3 * chargePower), hits[i].gameObject, PlayerActions.WeaponHit);
            }
        }
    }
    public override void OnLateUpdate()
    {
        if (PlayerStatus.Inst.IsStunned || !weapon.IsSelected)
            return;

        // Look At Mouse
        transform.LookAtMouseFlipX(Global.Inst.MainCam, transform);
    }

    private void OnAnimEnd_HeavyPunch()
    {
        PunchAnimEnd = true;
    }
}
