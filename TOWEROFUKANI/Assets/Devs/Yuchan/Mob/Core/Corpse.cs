using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dongjun.Helper;

public class Corpse : PoolingObj
{
    [SerializeField] Animator _ani;

    public void Init(CorpseData d)
    {
        name = Prefab.name;
        _ani.Play(gameObject.name, 0, d.mFCntMin != d.mFCntMax ? Random.Range(d.mFCntMin, d.mFCntMax) / (float)d.mFCntMax : d.mFCntMin);
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
    private void Start()
    {

    }

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

    bool mAbsorb = false;
    public void StartAbsorb()
    {
        mAbsorb = true;
        GetComponent<Collider2D>().enabled = false;
        rb2D.gravityScale = 0;
    }


    float speed = 20;
    Vector2 cScale = Vector2.one;
    private void Update()
    {
        if (!mAbsorb) return;
        // 시체조각 흡수시 작아지는 연출....
        transform.position = Vector2.MoveTowards(transform.position, GM.PlayerPos, speed * Time.deltaTime);
        transform.localScale = Vector3.one * Vector2.Distance(GM.PlayerPos, transform.position) * 0.5f;
        transform.localScale = transform.localScale.Clamp(Vector2.zero, cScale);
        cScale = transform.localScale;
        if (Vector2.Distance(GM.PlayerPos, transform.position) <= 0.1f)
        {
            this.Sleep();
        }
    }
}