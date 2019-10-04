using UnityEngine;

public abstract class GunAction_Base<TGunMain> : CLA_Action 
    where TGunMain : Gun
{ 
    protected Animator animator;
    protected TGunMain gun;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        gun = GetComponent<TGunMain>();
    }
}
