﻿using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_Action : CLA_Action,
    ICanDetectGround
{
    #region Var: Inspector
    [Header("Visual")]
    [SerializeField]
    private Transform spriteRoot;

    [Header("Collision")]
    [SerializeField]
    private BoxCollider2D oneWayCollider;

    [Header("GroundDetection")]
    [SerializeField]
    private GroundDetectionData groundDetectionData;

    [Header("Gravity")]
    [SerializeField]
    private GravityData gravityData;

    [Header("Walk")]
    [SerializeField]
    private float walkSpeed = 6f;

    [Header("Jump")]
    [SerializeField]
    private JumpData jumpData;
    #endregion

    #region Var: Ground Detection
    private bool isGrounded = false;
    private GroundInfo curGroundInfo = new GroundInfo();
    #endregion

    #region Var: Jump
    private bool jumpKeyPressed = false;
    private bool isJumping = false;
    #endregion

    #region Var: Fall Through
    private bool fallThroughKeyPressed = false;
    #endregion

    #region Var: Components
    private Animator animator;
    private Rigidbody2D rb2D;
    #endregion


    #region Method: Unity
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    #endregion

    #region Method: Action
    public override void OnEnd()
    {
        // Reset Ground Data
        isGrounded = false;
        curGroundInfo.Reset();

        // Reset Jump Data
        isJumping = false;

        // Reset Velocity
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
    }
    public override void OnUpdate()
    {
        // Update Ground Detection Size
        groundDetectionData.Size = oneWayCollider.size * transform.localScale;
    }
    public override void OnLateUpdate()
    {
        // Look At Mouse
        LookAtMouse_Logic.FlipY(CommonObjs.Inst.MainCam, spriteRoot, transform);
    }
    public override void OnFixedUpdate()
    {
        // Animation
        UpdateAnimation();

        // Detect Ground
        GroundDetection_Logic.DetectGround(!isJumping, rb2D, transform, groundDetectionData, ref isGrounded, ref curGroundInfo);
        GroundDetection_Logic.ExecuteOnGroundMethod(this, isGrounded, ref groundDetectionData);

        // Fall Through
        fallThroughKeyPressed = PlayerInputManager.Inst.Input_FallThrough;
        GroundDetection_Logic.FallThrough(
            ref fallThroughKeyPressed,
            isGrounded, 
            rb2D,
            transform, 
            oneWayCollider, 
            groundDetectionData);

        // Walk
        rb2D.velocity = new Vector2(walkSpeed * PlayerInputManager.Inst.Input_WalkDir, rb2D.velocity.y);

        // Jump
        jumpKeyPressed = PlayerInputManager.Inst.Input_Jump;
        Jump_Logic.Jump(ref jumpKeyPressed, ref isJumping, ref jumpData, rb2D, transform);

        // Gravity
        Gravity_Logic.ApplyGravity(rb2D, isGrounded ? new GravityData(0, 0) : !isJumping ? gravityData : new GravityData(jumpData.jumpGravity, 0));
    }
    #endregion

    #region Method: Animation
    private void UpdateAnimation()
    {
        if (isGrounded)
        {
            if (PlayerInputManager.Inst.Input_WalkDir == 0)
            {
                animator.Play("Player_Idle");
            }
            else
            {
                if ((PlayerInputManager.Inst.Input_WalkDir == 1 && spriteRoot.rotation.eulerAngles.y == 0) ||
                    (PlayerInputManager.Inst.Input_WalkDir == -1 && spriteRoot.rotation.eulerAngles.y == 180))
                    animator.Play("Player_Walk_Forward");
                else
                    animator.Play("Player_Walk_Backward");
            }
        }
        else
        {
            if (!isJumping)
            {
                animator.Play("Player_Airborne");
            }
            else
            {
                animator.Play("Player_Jump");

                if (jumpData.count_Cur > 0 && PlayerInputManager.Inst.Input_Jump)
                    animator.Play("Player_Jump", 0, 0f);
            }
        }
    }
    #endregion

    #region Interface: ICanDetectGround
    public void OnGroundEnter()
    {
        // Reset Jump
        Jump_Logic.ResetJump(ref jumpData);
    }
    public void OnGroundStay()
    {
    }
    public void OnGroundExit()
    {
    }
#endregion
}