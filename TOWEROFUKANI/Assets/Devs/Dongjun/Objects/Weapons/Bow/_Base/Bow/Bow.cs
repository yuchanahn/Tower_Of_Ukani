using UnityEngine;

public abstract class Bow : Weapon
{
    [Header("Timer")]
    [SerializeField] public TimerData shootTimer;
    [SerializeField] public TimerData drawTimer;

    public bool canShoot = false;
    public float drawPower = 0;

    protected override void Start()
    {
        base.Start();

        // Init Timer
        shootTimer.Init(gameObject);
        drawTimer.Init(gameObject);
    }
}
