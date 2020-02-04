using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dongjun.Helper;
using System;

public class Corpse : PoolingObj
{
    Animator mAni;
    eCorpseSpawnMode mMode = eCorpseSpawnMode.None;
    Action<GameObject> mCoprseModeEvent = null;
    Dictionary<eCorpseSpawnMode, Action> mModeAction = new Dictionary<eCorpseSpawnMode, Action>();


    private void Awake()
    {
        mAni = GetComponentInChildren<Animator>();
        mModeAction[eCorpseSpawnMode.Absorb] = absorb;
        mModeAction[eCorpseSpawnMode.Destroy] = destory;
        mModeAction[eCorpseSpawnMode.None] = ()=> {};
    }

    public void Init(CorpseData d)
    {
        name = Prefab.name;
        mMode = d.CurrentMode;
        mCoprseModeEvent = d.ModeEvent;
        mAni.Play(gameObject.name, 0, UnityEngine.Random.Range(0f,1f));
    }

    public void DestroyOfTime(float t)
    {
        ATimer.Set(GetInstanceID().ToString(), t, this.Sleep);
    }
    private void OnDestroy()
    {
        ATimer.Pop(GetInstanceID().ToString());
    }

    Rigidbody2D rb2D = null;
    Collider2D col;

    float GravityScale = 0;


    public override void ResetOnSpawn()
    {
        if (rb2D == null)
        {
            rb2D = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            GravityScale = rb2D.gravityScale;
        }
        cScale = Vector2.one;
        col.enabled = true;
        rb2D.isKinematic = false;
        rb2D.velocity = new Vector2(0, 0);
        rb2D.gravityScale = GravityScale;
        transform.localScale = Vector3.one;
    }


    float speed = 20;
    Vector2 cScale = Vector2.one;
    bool StartAbsord = false;

    void absorb()
    {
        if(!StartAbsord)
        {
            StartAbsord = true;
            GetComponent<Collider2D>().enabled = false;
            rb2D.gravityScale = 0;
        }

        transform.position = Vector2.MoveTowards(transform.position, GM.PlayerPos, speed * Time.deltaTime);
        transform.localScale = Vector3.one * Vector2.Distance(GM.PlayerPos, transform.position) * 0.5f;
        transform.localScale = transform.localScale.Clamp(Vector2.zero, cScale);
        cScale = transform.localScale;
        if (Vector2.Distance(GM.PlayerPos, transform.position) <= 0.1f)
        {
            this.Sleep();
            mCoprseModeEvent(Prefab.GetComponent<Corpse>().gameObject);
        }
        
    }
    void destory()
    {
        //mCoprseModeEvent(Prefab.GetComponent<Corpse>().gameObject);
    }
    
    private void Update()
    {
        mModeAction[mMode]();
    }
}