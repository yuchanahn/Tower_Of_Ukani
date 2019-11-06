﻿using UnityEngine;

public static class Anim_Logic
{
    public static void SetAnimSpeed(Animator animator, float duration, string animName = null)
    {
        if ((animName != null && !animator.CheckCurAnimName(animName)) || duration <= 0)
        {
            animator.speed = 1;
            return;
        }

        animator.speed = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / duration;
    }
    public static void SetAnimSpeed(Animator animator, float duration, float maxDuration, string animName = null)
    {
        if ((animName != null && !animator.CheckCurAnimName(animName)) || duration <= 0)
        {
            animator.speed = 1;
            return;
        }

        
        animator.speed = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / (maxDuration <= 0 || duration < maxDuration ? duration : maxDuration);
    }
    public static void ResetAnimSpeed(Animator animator)
    {
        animator.speed = 1;
    }

    public static bool CheckCurAnimName(this Animator animator, string nameToCompare)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(nameToCompare);
    }
    public static float GetNormalizedTime(this Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}