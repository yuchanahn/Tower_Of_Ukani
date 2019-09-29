using UnityEngine;

public class SelfSleepObj : PoolingObj
{
    [SerializeField] private TimerData sleepTimer;

    public override void ResetOnActive()
    {
        sleepTimer.curTime = 0;
        sleepTimer.Restart();
    }

    protected virtual void Start()
    {
        sleepTimer.Init(gameObject, OnTimerMax: Sleep);
    }
    protected virtual void Sleep()
    {
        ObjPoolingManager.Sleep(this);
    }
}
