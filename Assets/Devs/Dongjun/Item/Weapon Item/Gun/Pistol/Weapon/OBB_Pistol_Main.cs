using Dongjun.Helper;
using UnityEngine;

public class OBB_Pistol_Main : AimedWeapon_State_Base<OBB_Data_Pistol, PistolItem>
{
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Bullet bulletPrefab;

    [Header("Shoot Animation")]
    [SerializeField] private float shootAnimMaxDur;

    [Header("Muzzle Flash")]
    [SerializeField] private PoolingObj muzzleFlashPrefab;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;

    public override void OnEnter()
    {
        // Shoot
        SpawnBullet();
        VisualEffect();

        // Timer
        weaponItem.Timer_Shoot.Reset();

        // Animation
        data.Animator.Play(weaponItem.ANIM_Shoot);

        // Trigger Item Effect
        PlayerActionEventManager.Trigger(PlayerActions.WeaponMain);
        PlayerActionEventManager.Trigger(PlayerActions.GunShoot);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Timer_Shoot.EndTime.Value, shootAnimMaxDur);
    }
    public override void OnExit()
    {
        // Animation
        data.Animator.ResetSpeed();
    }

    private void SpawnBullet()
    {
        // Spawn Bullet
        Bullet bullet = bulletPrefab.Spawn(shootPoint.position, transform.rotation);

        // Set Bullet Data
        bullet.InitData(bullet.transform.right, weaponItem.BulletData, weaponItem.AttackData);

        // Consume Bullet
        weaponItem.UseBullet(1);
    }
    private void VisualEffect()
    {
        // Muzzle Flash
        muzzleFlashPrefab.Spawn(shootPoint, new Vector2(0, 0), Quaternion.identity);

        // Cam Shake
        CamShake_Logic.ShakeDir(camShakeData_Shoot, transform, Vector2.left);
    }
}
