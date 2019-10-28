using UnityEngine;

public abstract class BowItem : WeaponItem
{
    [SerializeField] protected GameObject arrowVisual;

    public TimerStat shootTimer;
    public TimerStat drawTimer;

    [HideInInspector] public bool hasBeenDrawn = false;
    [HideInInspector] public float drawPower = 0;

    public GameObject ArrowVisual => arrowVisual;

    protected void Start()
    {
        shootTimer.Init(gameObject);
        drawTimer.Init(gameObject);
    }
}

public abstract class BowObject : WeaponObject<BowItem>
{

}
