public class CLA_Action<TMain> : CLA_Action_Base 
    where TMain : CLA_Main
{
    protected TMain main;

    protected virtual void Awake()
    {
        main = GetComponent<TMain>();
    }
}
