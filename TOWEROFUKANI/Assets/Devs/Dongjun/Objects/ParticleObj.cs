using UnityEngine;

public class ParticleObj : PoolingObj
{
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private TimerData destroyTimer;

    private void Start()
    {
        destroyTimer.Init(gameObject, OnTimerMax: DisableOnFinish);
    }
    public override void ResetOnActive()
    {
        destroyTimer.Restart();

        for (int i = 0; i < particleSystems.Length; i++)
            particleSystems[i].Play();
    }

    private void DisableOnFinish()
    {
        ObjPoolingManager.Sleep(this);
    }
}
