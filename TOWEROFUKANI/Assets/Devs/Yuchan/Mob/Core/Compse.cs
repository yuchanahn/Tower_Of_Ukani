using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compse : Object_ObjectPool<Compse>
{
    [SerializeField] Animator _ani;

    public override void ThisStart()
    {
        var rb2D = GetComponent<Rigidbody2D>();
        rb2D.isKinematic = false;
        rb2D.velocity = new Vector2(0, 0);
    }

    public void Init(CorpseData d)
    {
        _ani.Play(gameObject.name, 0, d.mFCntMin != d.mFCntMax ? (float)Random.Range(d.mFCntMin, d.mFCntMax) / (float)d.mFCntMax : d.mFCntMin);
    }
}