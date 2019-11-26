using UnityEngine;

public abstract class BowShoot_Base<TItem> : BowAction_Base<TItem>
    where TItem : BowItem
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

    #region Method: CLA_Action
    public override void OnEnter()
    {
        weapon.shootTimer.SetActive(true);
        weapon.shootTimer.Restart();

        animator.Play(weapon.ANIM_Shoot, 0, 0);
    }
    public override void OnExit()
    {
        weapon.shootTimer.SetActive(false);
        weapon.shootTimer.ToZero();

        animator.ResetSpeed();
    }
    public override void OnLateUpdate()
    {
        if (!weapon.IsSelected)
            return;

        // Look At Mouse
        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, weapon.SpriteRoot.transform, transform);

        // Animation Speed
        animator.SetSpeed(weapon.shootTimer.EndTime.Value, maxShootAnimTime, weapon.ANIM_Shoot);
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        SpawnArrow();
        ShootEffects();

        // Trigger Item Effect
        ItemEffectManager.Trigger(PlayerActions.WeaponMain);
        ItemEffectManager.Trigger(PlayerActions.BowShoot);
    }
    private void SpawnArrow()
    {
        // Set Attack Data
        AttackData curAttackData = weapon.attackData;
        curAttackData.damage = new IntStat(Mathf.Max(MathD.Round(weapon.attackData.damage.Value * weapon.drawPower), 1));

        // Set Projectile Data
        ProjectileData curArrowData = weapon.arrowData;
        curArrowData.moveSpeed.Base *= weapon.drawPower;

        // Spawn Arrow
        Arrow arrow = arrowPrefab.Spawn(shootPoint.position, transform.rotation);

        // Set Arrow Data
        arrow.InitData(curArrowData, curAttackData);
    }
    private void ShootEffects()
    {
        // Cam Shake
        CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
    }
    #endregion

    #region Method: Anim Event
    private void OnAnim_ShootArrow()
    {
        Shoot();
    }
    #endregion
}
