using UnityEngine;

public class OBB_Data_Animator : OBB_Data
{
    public Animator Animator
    { get; private set; }

    public override void Init_Awake(GameObject gameObject)
    {
        Animator = gameObject.GetComponent<Animator>();
    }
    public override void Init_Start(GameObject gameObject)
    {

    }
}
