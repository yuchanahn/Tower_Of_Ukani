using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropObj : SelfSleepObj
{
    protected Rigidbody2D rb2D;

    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    public override void OnSpawn()
    {
        base.OnSpawn();
        rb2D.velocity = GM.Player.Data.RB2D.velocity;
    }
}
