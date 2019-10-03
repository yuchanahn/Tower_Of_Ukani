using UnityEngine;

public class GunAction_Base<GunMain> : CLA_Action 
    where GunMain : Gun
{ 
    protected Animator animator;
    protected GunMain gun;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        gun = GetComponent<GunMain>();
    }
}
