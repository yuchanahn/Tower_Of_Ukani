using UnityEngine;

public class MachineGun_EmptyShell : PlayerDropObj
{
    [Header("Force")]
    [SerializeField] private float forceMin;
    [SerializeField] private float forceMax;

    [Header("Change Order In Layer Timer")]
    [SerializeField] private TimerData showBehindObjTimer;

    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    protected override void Start()
    {
        base.Start();

        showBehindObjTimer
            .SetTick(gameObject)
            .SetAction(onEnd: () => spriteRenderer.sortingOrder = -100);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();

        rb2D.AddForce((Vector3.up - transform.right).normalized * Random.Range(forceMin, forceMax), ForceMode2D.Impulse);
        rb2D.AddTorque(Mathf.Sign(transform.right.x) * Random.Range(forceMin, forceMax), ForceMode2D.Impulse);

        showBehindObjTimer.SetTick(gameObject);
        showBehindObjTimer.Reset();

        spriteRenderer.sortingOrder = 100;
    }
    protected override void SelfSleep()
    {
        base.SelfSleep();

        showBehindObjTimer.SetTick(gameObject, TickMode.Manual);
    }
}
