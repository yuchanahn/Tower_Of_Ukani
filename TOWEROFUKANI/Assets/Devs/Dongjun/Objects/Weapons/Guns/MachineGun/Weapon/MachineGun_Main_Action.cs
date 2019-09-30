using UnityEngine;

public class MachineGun_Main_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Shoot")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private PoolingObj bulletPrefab;

    [Header("Accuracy")]
    [SerializeField] private float acry_YPosOffset;
    [SerializeField] private float acry_ZRotOffset;

    [Header("Animation")]
    [SerializeField] private float maxShootAnimTime;

    [Header("Muzzle Flash")]
    [SerializeField] private Transform shootParticleParent;
    [SerializeField] private PoolingObj shootParticlePrefab;

    [Header("Empty Shell")]
    [SerializeField] private Transform emptyShellSpawnPos;
    [SerializeField] private PoolingObj emptyShellPrefab;

    [Header("Ammo Belt")]
    [SerializeField] private Transform ammoBelt;
    [SerializeField] private float ammoBeltAmmoCount;

    [Header("Camera Shake")]
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Components
    private Animator animator;
    private MachineGun gun_Main;
    #endregion


    #region Method: Unity
    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun_Main = GetComponent<MachineGun>();
    }
    #endregion

    #region Method: CLA_Action
    public override void OnStart()
    {
        if (gun_Main.gunData.loadedBullets == gun_Main.gunData.magazineSize)
            ammoBelt.localPosition = Vector3.zero;
    }
    public override void OnEnd()
    {
        animator.speed = 1;
        animator.ResetTrigger("Shoot");
    }
    public override void OnUpdate()
    {
        if (!gun_Main.IsSelected)
            return;

        if (gun_Main.gunData.shootTimer.IsTimerAtMax && Input.GetKey(PlayerInputManager.Inst.Keys.MainAbility))
        {
            // Continue Timer
            gun_Main.gunData.shootTimer.Restart();

            // Spawn Bullet
            Transform bullet = ObjPoolingManager.Activate(bulletPrefab, shootPoint.position, transform.rotation).transform;
            bullet.position += shootPoint.up * Random.Range(-acry_YPosOffset, acry_YPosOffset);
            bullet.rotation = Quaternion.Euler(0, 0, bullet.eulerAngles.z + Random.Range(-acry_ZRotOffset, acry_ZRotOffset));

            // Use Bullet
            gun_Main.gunData.loadedBullets -= 1;

            // Animation
            animator.SetTrigger("Shoot");

            // Update Ammo Belt Pos
            ammoBelt.localPosition = 
                new Vector2(0, Mathf.Lerp(0, 0.0625f * ammoBeltAmmoCount, 1 - ((float)gun_Main.gunData.loadedBullets / gun_Main.gunData.magazineSize)));

            // Particle Effect
            ObjPoolingManager.Activate(shootParticlePrefab, shootParticleParent, new Vector2(0, 0), Quaternion.identity).transform.position = bullet.position;

            // Cam Shake Effect
            CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
        }
    }
    public override void OnLateUpdate()
    {
        AnimSpeed_Logic.SetAnimSpeed(animator, gun_Main.gunData.shootTimer.endTime, maxShootAnimTime, gun_Main.WeaponNameTrimed + "_Shoot");
        LookAtMouse_Logic.Rotate(Global.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(Global.Inst.MainCam, gun_Main.SpriteRoot.transform, transform);
    }
    #endregion
}
