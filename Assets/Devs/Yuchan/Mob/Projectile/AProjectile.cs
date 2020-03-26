using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AProjectile<T> : Object_ObjectPool<AProjectile<T>>
{
    Vector2 mVel = Vector2.zero;
    [SerializeField] int Speed;
    [SerializeField] float DestroyT;
    [SerializeField] int Damage;

    public override void Begin()
    {
        base.Begin();
        DestroyObj(DestroyT);
    }

    public override void Init(Vector2 vel)
    {
        mVel = vel;
        transform.right = vel;
        if ((Vector2)transform.right == Vector2.left)
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
    }

    [Header("Object Detection")]
    [SerializeField] protected Vector2 detectSize;
    [SerializeField] protected LayerMask detectLayers;
    [SerializeField] protected string[] ignoreTags;

    protected bool IsIgnoreTag(Component target)
    {
        for (int j = 0; j < ignoreTags.Length; j++)
            if (target.CompareTag(ignoreTags[j]))
                return true;

        return false;
    }
    protected virtual bool CheckCreatureHit(RaycastHit2D hit2d)
    {
        PlayerStats.Inst.Damage(new AttackData(Damage));
        return true;
    }
    protected virtual void DetectObject()
    {
        Vector2 pos = transform.position + (transform.right * detectSize.x * 0.5f) - (transform.right * (Speed * Time.fixedDeltaTime));
        float rot = transform.rotation.eulerAngles.z;

        RaycastHit2D[] hits =
            Physics2D.BoxCastAll(pos, detectSize, rot, transform.right, Speed * Time.fixedDeltaTime, detectLayers);

        Vector2 hitPos = transform.position;
        GameObject hitObj = null;

        if (hits.Length == 0)
            return;

        bool hasHit = false;

        for (int i = 0; i < hits.Length; i++)
        {
            // Check Ignore Tag
            if (IsIgnoreTag(hits[i].collider))
                continue;

            // Set Data
            hasHit = true;
            hitPos = hits[i].point;
            hitObj = hits[i].collider.gameObject;

            if (hitObj.GetComponent<Creature>() != null)
            {
                if (CheckCreatureHit(hits[i]))
                {
                    break;
                }
                else
                {
                    hasHit = false;
                }
            }
        }

        if (hasHit)
        {
            OnHit(hitPos, hitObj);
        }
    }
    protected virtual void OnHit(Vector2 hitPos, GameObject hitObject)
    {
        DestroyObj();
    }
    private void Update()
    {
        transform.position += (Vector3)mVel * Speed * Time.deltaTime;
    }
    private void FixedUpdate()
    {
        DetectObject();
    }
}
