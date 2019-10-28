using UnityEngine;

public abstract class BowItem : WeaponItem
{
    // Visual
    [SerializeField] protected GameObject arrowVisual;

    // Timer
    public TimerStat shootTimer;
    public TimerStat drawTimer;

    // Arrow Data
    public WeaponProjectileData arrowData;

    // Extra Info
    [HideInInspector] public bool hasBeenDrawn = false;
    [HideInInspector] public float drawPower = 0;

    public GameObject ArrowVisual => arrowVisual;

    protected void Start()
    {
        shootTimer.Init(gameObject);
        drawTimer.Init(gameObject);
    }
}

public abstract class BowController : WeaponController<BowItem>
{

}
