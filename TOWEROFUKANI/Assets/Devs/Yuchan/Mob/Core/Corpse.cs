using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        col.enabled = true;
        rb2D.isKinematic = false;
        rb2D.velocity = new Vector2(0, 0);
        acc = 0;
        rb2D.gravityScale = GravityScale;
        transform.localScale = Vector3.one;
        StartAbsorb();
    }

    bool mAbsorb = false;
    public void StartAbsorb()
    {
        mAbsorb = true;
        GetComponent<Collider2D>().enabled = false;
        rb2D.gravityScale = 0;
    }


    float acc = 1;
    float aT = 0.1f;
    float speed = 2;
    private void Update()
    {
        if (!mAbsorb) return;
        // 시체조각 흡수시 작아지는 연출....
        var vel = (GM.PlayerPos - transform.position).normalized;
        transform.position += vel * Time.deltaTime * speed;
        transform.localScale = transform.localScale.x <= 0 ? Vector3.zero : transform.localScale - (Vector3.one * acc * Time.deltaTime);
        
        if (Vector2.Distance(GM.PlayerPos, transform.position) <= 0.1f)
        {
            this.Sleep();
        }
        acc += aT * Time.deltaTime;
    }
}