using UnityEngine;

public class WoodenShortBow_Main_Action : BowAction_Base<WoodenShortBow>
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Arrow arrowPrefab;
    [SerializeField] private WeaponProjectileData projectileData;

    [Header("Shoot Animation")]
    [SerializeField] private float maxShootAnimTime;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Current Data
    private WeaponProjectileData curProjectileData;
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

        weapon.arrowSprite.SetActive(weapon.shootTimer.IsEnded);

        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, weapon.SpriteRoot.transform, transform);
        Anim_Logic.SetAnimSpeed(animator, weapon.shootTimer.EndTime.Cur, maxShootAnimTime, string.Concat(weapon.Info.NameTrimed, "_Shoot"));
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        // Spawn Bullet
        Arrow arrow = arrowPrefab.Spawn(shootPoint.position, transform.rotation);

        curProjectileData = projectileData;
        curProjectileData.damage.Base = Mathf.Max(Mathf.RoundToInt(curProjectileData.damage.Base * weapon.drawPower), 1);
        curProjectileData.moveSpeed.Base *= weapon.drawPower;

        arrow.SetData(curProjectileData);
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
