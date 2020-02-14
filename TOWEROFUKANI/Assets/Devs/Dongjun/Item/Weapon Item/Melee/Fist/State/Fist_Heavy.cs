using UnityEngine;

public class Fist_Heavy : Melee_State_Base<FistItem>
{
    #region Var: Inspector
    [Header("Damage")]
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private Transform damagePoint;
    [SerializeField] private Vector2 damageSize;

    [Header("Effect")]
    [SerializeField] private CameraShake.Data camShakeData_Punch;
    #endregion

    #region Var: Heavy Attack
    private float chargeTime = 0;
    private bool chargeDone = false;
    #endregion

    #region Var: Status
    private PlayerStatus_Slow status_Slow;
    #endregion

    #region Prop: 
    public bool PunchAnimEnd
    { get; private set; } = false;
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(damagePoint.position, damageSize);
    }
    protected override void Awake()
    {
        base.Awake();
        status_Slow = new PlayerStatus_Slow(GM.Player.StatusID, GM.Player.gameObject, 50f);
    }

    public override void OnEnter()
    {
        // Status Effect
        PlayerStatus.AddEffect(status_Slow);

        // Trigger Item Effect
        ActionEffectManager.Trigger(PlayerActions.MeleeHeavyAttack);

        // Player
        GM.Player.CanDash = false;
        GM.Player.CanKick = false;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, true);
    }
    public override void OnExit()
    {
        chargeTime = 0;
        chargeDone = false;
        PunchAnimEnd = false;

        // Status Effect
        PlayerStatus.RemoveEffect(status_Slow);

        // Player
        GM.Player.CanDash = true;
        GM.Player.CanKick = true;
        GM.Player.CanChangeDir = true;
        PlayerInventoryManager.weaponHotbar.LockSlots(this, false);
    }
    public override void OnUpdate()
    {
        if (!weapon.IsSelected)
            return;

        if (PlayerStatus.IsHardCCed)
            return;

        // Charge
        if (!chargeDone)
        {
            chargeTime += Time.deltaTime;
            weapon.attackData_Heavy.damage.ModFlat = chargeTime * weapon.heavyChargePerSec;

            // Animation
            weapon.animator.Play("Heavy_Charge");
        }

        // Punch
        if (!chargeDone && Input.GetKeyUp(PlayerWeaponKeys.SubAbility))
        {
            chargeDone = true;

            // Deal Damage
            var hits = Physics2D.OverlapBoxAll(damagePoint.position, damageSize, 0f, damageLayer);
            for (int i = 0; i < hits.Length; i++)
            {
                PlayerStats.Inst.DealDamage(weapon.attackData_Heavy, hits[i].gameObject,
                    PlayerActions.WeaponHit,
                    PlayerActions.MeleeHeavyHit);
            }

            // Reset Charged Damage
            weapon.attackData_Heavy.damage.ModFlat = 0;

            // Status Effect
            PlayerStatus.RemoveEffect(status_Slow);

            // Animation
            weapon.animator.Play("Heavy_Punch");

            // Effect
            CamShake_Logic.ShakeDir(camShakeData_Punch, transform, Vector2.right);

            // Player
            GM.Player.CanChangeDir = false;
        }
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        if (PlayerStatus.IsHardCCed)
            return;

        // Look At Mouse
        if (!chargeDone)
            transform.LookAtMouseFlipX(Global.Inst.MainCam, transform);
    }

    private void OnAnimEnd_HeavyPunch()
    {
        PunchAnimEnd = true;
    }
}
