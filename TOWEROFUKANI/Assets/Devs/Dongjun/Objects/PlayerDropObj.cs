using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropObj : SelfSleepObj
{
    Rigidbody2D rb2D;

    public override void ResetOnActive()
    {
        base.ResetOnActive();

        if (rb2D != null)
            rb2D.velocity = CommonObjs.Inst.PlayerRB2D.velocity;
    }
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
}
