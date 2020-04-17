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

    public override void OnEnter()
    {
        // Timer
        weaponItem.Main_Shoot_CD.SetActive(true);

        // Animation
        data.Animator.Play(weaponItem.ANIM_Shoot);
    }
    public override void OnLateEnter()
    {
        // Animation
        data.Animator.SetDuration(weaponItem.Main_Shoot_CD.EndTime.Value, shootAnimMaxDur);
    }
    public override void OnExit()
    {
        // Timer
        weaponItem.Main_Shoot_CD.SetActive(false);
        weaponItem.Main_Shoot_CD.Reset();

        // Animation
        data.Animator.ResetSpeed();
    }

    private void SpawnArrow()
    {
        // Check Wall
        if (!ShootCheckWall_Logic.CanShoot(transform, shootPoint))
            return;

        // Set Attack Data
        AttackData curAttackData = weaponItem.AttackData;
        curAttackData.damage = new FloatStat(Mathf.Max(weaponItem.AttackData.damage.Value * weaponItem.DrawPower, 1));

        // Set Projectile Data
        ProjectileData curArrowData = weaponItem.Main_ArrowData;
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
