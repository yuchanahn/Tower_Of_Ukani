using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADeadBody : Object_ObjectPool<ADeadBody>
{
    [SerializeField] TimerData _timerData;
    [SerializeField] Animator _ani;
    [SerializeField] AnimationClip _clip;
    [SerializeField] int _count;
    [SerializeField] int _FrameNum;

    [SerializeField] int _countRangeMin;
    [SerializeField] int _countRangeMax;

    [SerializeField] bool bCountRandom;

    [SerializeField] int _FrameRangeMin;
    [SerializeField] int _FrameRangeMax;
    [SerializeField] bool bStartRandom;

    [SerializeField] int _aniStartFrame;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Vector2 size;


    void Awake()
    {
        _timerData.Init(gameObject, OnTimerMax : DestroyObj);
        _timerData.SetActive(false);
    }

    public int Count
    {
        get { return bCountRandom ? Random.Range(_countRangeMin, _countRangeMax + 1) : Count; }
        set => _count = value;
    }

    public override void ThisStart()
    {
        var rb2D = GetComponent<Rigidbody2D>();
        rb2D.isKinematic = false;
        rb2D.velocity = new Vector2(0, 0);
    }

    public void DBYStart()
    {
        float nomalType = 0;

        if (bStartRandom) nomalType = (float)Random.Range(_FrameRangeMin, _FrameRangeMax) / (float)_FrameRangeMax;
        else nomalType = (float)_FrameNum / (float)_FrameRangeMax;

        _ani.Play(_clip.name, 0, nomalType);
    }
}
