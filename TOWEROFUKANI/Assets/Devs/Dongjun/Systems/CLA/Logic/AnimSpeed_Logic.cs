using UnityEngine;

public static class AnimSpeed_Logic
{
    public static void SetAnimSpeed(Animator animator, float duration, string animName = null)
    {
        if (animName != null && animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != animName)
        {
            animator.speed = 1;
            return;
        }

        if (duration <= 0)
            return;

        animator.speed = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / duration;
    }
    public static void SetAnimSpeed(Animator animator, float duration, float maxDuration, string animName = null)
    {
        if (animName != null && animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != animName)
        {
            animator.speed = 1;
            return;
        }

        if (duration <= 0 || maxDuration <= 0)
            return;

        animator.speed = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length / (duration > maxDuration ? maxDuration : duration);
    }
}
