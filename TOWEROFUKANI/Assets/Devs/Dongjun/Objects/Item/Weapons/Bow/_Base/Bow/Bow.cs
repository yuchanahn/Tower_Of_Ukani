using UnityEngine;
using NaughtyAttributes;

public abstract class Bow : Weapon
{
    [BoxGroup("Timer")] [SerializeField] public TimerStat shootTimer;
    [BoxGroup("Timer")] [SerializeField] public TimerStat drawTimer;

    [BoxGroup("Visual")] [SerializeField] public GameObject arrowSprite;

    [HideInInspector] public bool hasBeenDrawn = false;
    [HideInInspector] public float drawPower = 0;

    protected override void Start()
    {
        base.Start();

        // Init Timer
        shootTimer.Init(gameObject);
        drawTimer.Init(gameObject);
    }
}
