using UnityEngine;

public abstract class MonoAnimator : CLA_Action
{
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
}