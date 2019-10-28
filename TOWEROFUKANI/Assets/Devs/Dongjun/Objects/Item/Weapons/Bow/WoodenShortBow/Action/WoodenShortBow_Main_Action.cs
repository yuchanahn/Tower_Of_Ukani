using UnityEngine;

public class WoodenShortBow_Main_Action : BowAction_Base<WoodenShotBowItem>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Arrow arrowPrefab;

    [Header("Shoot Animation")]
    [SerializeField] private float maxShootAnimTime;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Stats
    private WeaponProjectileData arrowData;
    #endregion

    #region Method: Unity
    protected override void Awake()
    {
        base.Awake();

        arrowData.damage = new IntStat(5, min: 0);
        arrowData.moveSpeed = new FloatStat(30f, min: 0f);
        arrowData.gravity = new FloatStat(1f, min: 0f);
        arrowData.maxTravelDist = new FloatStat(30f, min: 0f);
    }
    #endregion

    #region Method: CLA_Action
    public override void OnEnter()
    {
        weapon.shootTimer.SetActive(true);
        weapon.shootTimer.Restart();

        if (weapon.hasBeenDrawn)
        {
            weapon.hasBeenDrawn = false;
            AnimPlay_Shoot();
        }
    }
    public override void OnExit()
    {
        OnAnimEnd_Shoot();
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        weapon.ArrowVisual.SetActive(weapon.shootTimer.IsEnded);

        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, weapon.SpriteRoot.transform, transform);
        Anim_Logic.SetAnimSpeed(animator, weapon.shootTimer.EndTime.Value, maxShootAnimTime, string.Concat(weapon.Info.NameTrimed, "_Shoot"));
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        // Set Arrow Data
        arrowData = weapon.arrowData;
        arrowData.damage.Base = Mathf.Max(Mathf.RoundToInt(arrowData.damage.Base * weapon.drawPower), 1);
        arrowData.moveSpeed.Base *= weapon.drawPower;

        // Spawn Bullet
        Arrow arrow = arrowPrefab.Spawn(shootPoint.position, transform.rotation);
        arrow.SetData(arrowData);

        // Trigger Item Effect
        ItemEffectManager.Trigger(PlayerActions.WeaponMain);
        ItemEffectManager.Trigger(PlayerActions.BowShoot);
    }
    private void ShootEffects()
    {
        // Cam Shake
        CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
    }
    private void AnimPlay_Shoot()
    {
        animator.Play(string.Concat(weapon.Info.NameTrimed, "_Shoot"), 0, 0);
    }
    #endregion

    #region Method: Anim Event
    private void OnAnim_ShootArrow()
    {
        Shoot();
        ShootEffects();
    }
    private void OnAnimEnd_Shoot()
    {
        animator.speed = 1;
    }
    #endregion
}
