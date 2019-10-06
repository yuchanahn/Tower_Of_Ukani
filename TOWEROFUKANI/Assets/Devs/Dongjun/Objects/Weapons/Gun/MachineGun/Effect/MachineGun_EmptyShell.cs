﻿using UnityEngine;

public class MachineGun_EmptyShell : PlayerDropObj
{
    [Header("Force")]
    [SerializeField] private float forceMin;
    [SerializeField] private float forceMax;

    [Header("Order In Layer Timer")]
    [SerializeField] private TimerData showBehindObjTimer;

    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public override void ResetOnSpawn()
    {
        base.ResetOnSpawn();

        rb2D.AddForce((Vector3.up - transform.right).normalized * Random.Range(forceMin, forceMax), ForceMode2D.Impulse);
        rb2D.AddTorque(Mathf.Sign(transform.right.x) * Random.Range(forceMin, forceMax), ForceMode2D.Impulse);

        showBehindObjTimer.UseAutoTick(gameObject, true);
        showBehindObjTimer.SetAction(OnEnd: ShowBehindObj);
        showBehindObjTimer.Restart();

        spriteRenderer.sortingOrder = 1;
    }
    protected override void Sleep()
    {
        base.Sleep();

        showBehindObjTimer.UseAutoTick(gameObject, false);
    }

    private void ShowBehindObj()
    {
        spriteRenderer.sortingOrder = -1;
    }
}