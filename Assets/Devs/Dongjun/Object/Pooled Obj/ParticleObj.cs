using UnityEngine;

public class ParticleObj : PoolingObj
{
    [SerializeField] private ParticleSystem[] particleSystems;
    [SerializeField] private TimerData destroyTimer;

    private void Start()
    {
        destroyTimer.SetTick(gameObject);
        destroyTimer.SetAction(onEnd: DisableOnFinish);
    }
    public override void OnSpawn()
    {
        destroyTimer.Reset();

        for (int i = 0; i < particleSystems.Length; i++)
            particleSystems[i].Play();
    }

    private void DisableOnFinish()
    {
        ObjPoolingManager.Sleep(this);
    }
}
