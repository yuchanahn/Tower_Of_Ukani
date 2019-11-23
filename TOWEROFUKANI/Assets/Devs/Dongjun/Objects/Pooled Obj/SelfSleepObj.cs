using UnityEngine;

public class SelfSleepObj : PoolingObj
{
    [Header("Sleep Timer")]
    public TimerData sleepTimer;

    protected virtual void Start()
    {
        sleepTimer.Init(gameObject, OnEnd: Sleep);
    }
    public override void ResetOnSpawn()
    {
        sleepTimer.CurTime = 0;
        sleepTimer.Restart();
    }

    protected virtual void Sleep()
    {
        ObjPoolingManager.Sleep(this);
    }
}
