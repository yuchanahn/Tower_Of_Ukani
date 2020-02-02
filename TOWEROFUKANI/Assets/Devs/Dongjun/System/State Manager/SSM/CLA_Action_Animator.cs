using UnityEngine;

public abstract class CLA_Action_Animator : SSM_State
{
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
}