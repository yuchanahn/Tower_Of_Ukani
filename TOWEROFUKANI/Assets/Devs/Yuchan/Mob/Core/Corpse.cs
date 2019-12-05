using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Corpse : PoolingObj
{
    [SerializeField] Animator _ani;

    public void Init(CorpseData d)
    {
        _ani.Play(gameObject.name, 0, d.mFCntMin != d.mFCntMax ? (float)Random.Range(d.mFCntMin, d.mFCntMax) / (float)d.mFCntMax : d.mFCntMin);
    }

    public void DestroyOfTime(float t)
    {
        ATimer.Set(GetInstanceID().ToString(), t, this.Sleep);
    }
    private void OnDestroy()
    {
        ATimer.Pop(GetInstanceID().ToString());
    }

    public override void ResetOnSpawn()
    {
        var rb2D = GetComponent<Rigidbody2D>();
        rb2D.isKinematic = false;
        rb2D.velocity = new Vector2(0, 0);
    }
}