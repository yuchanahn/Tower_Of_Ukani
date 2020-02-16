using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AProjectile : Object_ObjectPool<AProjectile>
{
    Vector2 mVel = Vector2.zero;
    int Speed;
    float DestroyT;

    public override void Begin()
    {
        base.Begin();
        DestroyObj(DestroyT);
    }

    public void Init(Vector2 vel, float destroyT)
    {
        mVel = vel;
        DestroyT = destroyT;
        transform.right = (transform.position + (Vector3)vel);
    }

    private void Update()
    {
        
        transform.position += (Vector3)mVel * Speed * Time.deltaTime;
    }
}
