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
        bow.shootTimer.SetActive(true);
        bow.shootTimer.Restart();

        if (bow.hasBeenDrawn)
        {
            bow.hasBeenDrawn = false;
            AnimPlay_Shoot();
        }
    }
    public override void OnExit()
    {
        OnAnimEnd_Shoot();
    }
    public override void OnLateUpdate()
    {
        if (!bow.IsSelected)
            return;

        bow.arrowSprite.SetActive(bow.shootTimer.IsEnded);

        LookAtMouse_Logic.AimedWeapon(Global.Inst.MainCam, bow.SpriteRoot.transform, transform);
        Anim_Logic.SetAnimSpeed(animator, bow.shootTimer.EndTime, maxShootAnimTime, string.Concat(bow.WeaponNameTrimed, "_Shoot"));
    }
    #endregion

    #region Method: Shoot
    private void Shoot()
    {
        // Spawn Bullet
        Arrow arrow = arrowPrefab.Spawn(shootPoint.position, transform.rotation);

        curProjectileData = projectileData;
        curProjectileData.damage = Mathf.Max(Mathf.RoundToInt(curProjectileData.damage * bow.drawPower), 1);
        curProjectileData.moveSpeed *= bow.drawPower;

        arrow.SetData(curProjectileData);
    }
    private void ShootEffects()
    {
        // Cam Shake
        CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
    }
    private void AnimPlay_Shoot()
    {
        animator.Play(string.Concat(bow.WeaponNameTrimed, "_Shoot"), 0, 0);
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
