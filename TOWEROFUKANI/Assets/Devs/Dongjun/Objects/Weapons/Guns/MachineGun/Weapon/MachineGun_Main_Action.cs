﻿using UnityEngine;

public class MachineGun_Main_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Points")]
    [SerializeField] private Transform shootPoint;

    [Header("Prefabs")]
    [SerializeField] private PoolingObj bulletPrefab;

    [Header("Animation")]
    [SerializeField] private float maxShootAnimTime;

    [Header("Effects")]
    [SerializeField] private Transform shootParticleParent;
    [SerializeField] private PoolingObj shootParticlePrefab;
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Properties
    public bool AnimEnd_Shoot { get; private set; } = false;
    #endregion

    #region Var: Components
    private Animator animator;
    private MachineGun gun_Main;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        gun_Main = GetComponent<MachineGun>();
    }

    public override void OnEnd()
    {
        AnimEnd_Shoot = false;
        animator.speed = 1;
        animator.ResetTrigger("Shoot");
    }
    public override void OnUpdate()
    {
        if (!gun_Main.IsSelected)
            return;

        if (gun_Main.Stats.shootTimer.IsTimerAtMax && Input.GetKey(PlayerInputManager.Inst.Keys.MainAbility))
        {
            // Spawn Bullet
            ObjPoolingManager.Activate(bulletPrefab, shootPoint.position, transform.rotation);

            // Use Bullet
            gun_Main.Stats.loadedBullets -= 1;

            // Continue Timer
            gun_Main.Stats.shootTimer.Continue();

            // Animation
            AnimEnd_Shoot = false;
            animator.SetTrigger("Shoot");

            // Particle Effect
            ObjPoolingManager.Activate(shootParticlePrefab, shootParticleParent, new Vector2(0, 0), Quaternion.identity);

            // Cam Shake Effect
            CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
        }
    }
    public override void OnLateUpdate()
    {
        AnimSpeed_Logic.SetAnimSpeed(animator, gun_Main.Stats.shootTimer.Timer_Max, maxShootAnimTime, "Pistol_Shoot");
        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, gun_Main.SpriteRoot.transform, transform);
    }

    private void OnAnimEnd_Shoot()
    {
        AnimEnd_Shoot = true;
    }
}
