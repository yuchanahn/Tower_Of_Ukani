using UnityEngine;

public class SelfSleepObj : PoolingObj
{
    [Header("Sleep Timer")]
    public TimerData sleepTimer;

    protected virtual void Start()
    {
        sleepTimer.SetTick(gameObject);
        sleepTimer.SetAction(onEnd: SelfSleep);
    }
    
    public override void OnSpawn()
    {
        sleepTimer.CurTime = 0;
        sleepTimer.Reset();
    }
    protected virtual void SelfSleep()
    {
        ObjPoolingManager.Sleep(this);
    }
}
