﻿using UnityEngine;

public class Shotgun_Main_Action : CLA_Action
{
    #region Var: Inspector
    [Header("Stats")]
    [SerializeField] private int pelletCount = 2;
    [SerializeField] private float pelletAngle = 10f;

    [Header("Points")]
    [SerializeField] private Transform shootPoint;

    [Header("Prefabs")]
    [SerializeField] private PoolingObj bulletPrefab;

    [Header("Effects")]
    [SerializeField] private float maxShootAnimTime;
    [SerializeField] private CameraShake.Data camShakeData_Shoot;
    #endregion

    #region Var: Components
    private Animator animator;
    private Shotgun shotgun_Main;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        shotgun_Main = GetComponent<Shotgun>();
    }

    public override void OnEnd()
    {
        animator.speed = 1;
    }
    public override void OnUpdate()
    {
        if (shotgun_Main.IsSelected && shotgun_Main.Stats.shootTimer.IsTimerAtMax)
        {
            if (Input.GetKeyDown(PlayerInputManager.Inst.Keys.MainAbility))
            {
                // Spawn Bullet
                Vector3 eRot = transform.eulerAngles;
                eRot.z -= ((pelletCount / 2) - (pelletCount % 2 == 0 ? 0.5f : 0)) * pelletAngle;
                for (int i = 0; i < pelletCount; i++)
                {
                    ObjPoolingManager.Activate(bulletPrefab, shootPoint.position, Quaternion.Euler(eRot));
                    eRot.z += pelletAngle;
                }

                // Use Bullet
                shotgun_Main.Stats.loadedBullets -= 1;

                // Continue Timer
                shotgun_Main.Stats.shootTimer.Continue();

                // Animation
                animator.SetTrigger("Shoot");

                // Cam Shake Effect
                CamShake_Logic.ShakeBackward(camShakeData_Shoot, transform);
            }
        }
    }
    public override void OnLateUpdate()
    {
        AnimSpeed_Logic.SetAnimSpeed(animator, shotgun_Main.Stats.shootTimer.Timer_Max, maxShootAnimTime, "Pistol_Shoot");

        LookAtMouse_Logic.Rotate(CommonObjs.Inst.MainCam, transform, transform);
        LookAtMouse_Logic.FlipX(CommonObjs.Inst.MainCam, shotgun_Main.SpriteRoot.transform, transform);
    }
}

