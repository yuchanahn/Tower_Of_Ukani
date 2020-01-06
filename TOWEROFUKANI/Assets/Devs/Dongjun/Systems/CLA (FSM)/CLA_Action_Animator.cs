using UnityEngine;

public abstract class CLA_Action_Animator : CLA_Action_Base
{
    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
}