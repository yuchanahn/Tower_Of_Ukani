using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObj : PoolingObj
{
    [SerializeField] private ParticleSystem[] particleSystem;
    [SerializeField] private TimerData destroyTimer;

    private void Start()
    {
        destroyTimer.Init(gameObject, OnTimerMax: DisableSelf);
    }
    protected override void ResetOnActive()
    {
        destroyTimer.Continue();

        for (int i = 0; i < particleSystem.Length; i++)
            particleSystem[i].Play();
    }

    private void DisableSelf()
    {
        ObjPoolingManager.Sleep(this);
    }
}
