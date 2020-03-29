using Dongjun.Helper;
using UnityEngine;

public class OBB_WoodenShortbow_Shoot : AimedWeapon_State_Base<OBB_Data_WoodenShortbow, WoodenShortbowItem>
{
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Arrow arrowPrefab;

    [Header("Shoot Animation")]
    [SerializeField] private float shootAnimMaxDur;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;

    private ShootCheckWallData shootCheckWallData = new ShootCheckWallData(0.8755f);

    public override void OnEnter()
    {
        // Timer
        weaponItem.Timer_Shoot.SetActive(true);

        // Animation
        data.Animator.Play(weaponItem.ANIM_Shoot);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Timer_Shoot.EndTime.Value, shootAnimMaxDur);
    }
    public override void OnExit()
    {
        // Timer
        weaponItem.Timer_Shoot.SetActive(false);
        weaponItem.Timer_Shoot.Reset();

        // Animation
        data.Animator.ResetSpeed();
    }

    private void SpawnArrow()
    {
        // Check Wall
        if (!shootCheckWallData.CanShoot(transform))
            return;

        // Set Attack Data
        AttackData curAttackData = weaponItem.AttackData;
        curAttackData.damage = new FloatStat(Mathf.Max(weaponItem.AttackData.damage.Value * weaponItem.DrawPower, 1));

        // Set Projectile Data
        ProjectileData curArrowData = weaponItem.arrowData;
        curArrowData.moveSpeed.Base *= weaponItem.DrawPower;

        // Spawn Arrow
        Arrow arrow = arrowPrefab.Spawn(shootPoint.position, transform.rotation);

        // Set Arrow Data
        arrow.InitData(arrow.transform.right, curArrowData, curAttackData);
    }
    private void VisualEffect()
    {
        // Cam Shake
        CamShake_Logic.ShakeDir(camShakeData_Shoot, transform, Vector2.left);
    }

    private void AnimEvent_ShootArrow()
    {
        SpawnArrow();
        VisualEffect();

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.WeaponMain);
        PlayerActionEventManager.Trigger(PlayerActions.BowShoot);
    }
}
